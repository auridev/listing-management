using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Helpers;
using Xunit;
using static LanguageExt.Prelude;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class ActiveListing_should : Listing_should
    {
        private readonly ActiveListing _sut;

        protected static readonly DateTimeOffset _expirationDate = DateTimeOffset.UtcNow.AddDays(90);

        public ActiveListing_should()
        {
            _sut = new ActiveListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                _expirationDate);
        }

        [Fact]
        public void have_an_ExpirationDate_property()
        {
            _sut.ExpirationDate.Should().BeCloseTo(_expirationDate);
        }

        [Fact(Skip = "this should be in command logic")]
        public void thrown_an_exception_during_creation_if_expiration_date_has_passed()
        {
            Action createAction = () => new ActiveListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow.AddDays(-90));

            createAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void thrown_an_exception_during_ActiveListing_creation_if_some_arguments_are_not_valid()
        {
            Action createAction = () => new ActiveListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                default);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void be_deactivatable()
        {
            // arrange
            Either<Error, TrimmedString> eitherReason = TrimmedString.Create("wrong number");
            DateTimeOffset deactivationDate = DateTimeOffset.UtcNow;

            // act
            eitherReason
                .Bind(reason => _sut.Deactivate(reason))
                .Right(passiveListing =>
                {
                    // assert
                    passiveListing.Should().NotBeNull();
                    passiveListing.Reason.Value.Should().Be("wrong number");
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void reject_to_deactivate_if_reason_is_null_arguments_are_not_valid()
        {
            Either<Error, PassiveListing> action = _sut.Deactivate(null);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Fact]
        public void be_able_to_receive_offers()
        {
            // arrange 
            var offerId = Guid.NewGuid();
            var createdDate = DateTimeOffset.UtcNow;
            Either<Error, (Owner owner, MonetaryValue monetaryValue)> combined =
                from o in Owner.Create(Guid.NewGuid())
                from mn in MonetaryValue.Create(1M, "usd")
                select (o, mn);

            // act
            Either<Error, Unit> action = combined
                .Map(combined => new ActiveOffer(offerId, combined.owner, combined.monetaryValue, createdDate))
                .Bind(offer => _sut.ReceiveOffer(offer));

            // assert
            action
                .Right(_ => _sut.ActiveOffers.Count.Should().Be(1))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void reject_null_offers()
        {
            // act
            Either<Error, Unit> action = _sut.ReceiveOffer(null);

            //assert
            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Message.Should().Be("invalid offer"));
        }

        [Fact]
        public void reject_offers_from_the_owner_of_the_listing()
        {
            // arrange
            var ownerId = Guid.NewGuid();
            var createdDate = DateTimeOffset.UtcNow;
            Either<Error, (Owner owner, MonetaryValue monetaryValue)> combined =
                from o in Owner.Create(ownerId)
                from mn in MonetaryValue.Create(34.89M, "usd")
                select (o, mn);

            // act
            Either<Error, Unit> action = combined
                .Bind(combined =>
                {
                    // create listing and offer with the same owner
                    var listing = new ActiveListing(Guid.NewGuid(),
                        combined.owner,
                        _listingDetails,
                        _contactDetails,
                        _locationDetails,
                        _geographicLocation,
                        _createdDate,
                        DateTimeOffset.UtcNow.AddDays(24));

                    var offer = new ActiveOffer(Guid.NewGuid(), combined.owner, combined.monetaryValue, createdDate);

                    return listing.ReceiveOffer(offer);
                });

            //assert
            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Message.Should().Be("cannot accept offers from the listing owner"));
        }

        [Fact]
        public void replace_previous_offer_of_an_owner_if_another_one_is_received()
        {
            // arrange
            Guid offerOwnerId = Guid.NewGuid();

            Either<Error, ActiveOffer> dummyOffer =
                from owner in Owner.Create(Guid.NewGuid())
                from value in MonetaryValue.Create(1M, "eur")
                select
                    new ActiveOffer(Guid.NewGuid(), owner, value, DateTimeOffset.UtcNow); // just an offer from some owner to increase the count

            Either<Error, ActiveOffer> offer1 =
                from owner in Owner.Create(offerOwnerId) // same owner
                from value in MonetaryValue.Create(6M, "eur")
                select
                    new ActiveOffer(Guid.NewGuid(), owner, value, DateTimeOffset.UtcNow);

            Either<Error, ActiveOffer> offer2 =
                from owner in Owner.Create(offerOwnerId) // same owner
                from value in MonetaryValue.Create(3M, "eur")
                select
                   new ActiveOffer(Guid.NewGuid(), owner, value, DateTimeOffset.UtcNow);

            // act
            Either<Error, Unit> addDummyOfferAction = dummyOffer
                .Bind(dummy => _sut.ReceiveOffer(dummy));

            Either<Error, Unit> addFirstOfferAction = offer1
                .Bind(offer1 => _sut.ReceiveOffer(offer1));

            Either<Error, Unit> addSecondOfferAction = offer2
                .Bind(offer2 => _sut.ReceiveOffer(offer2));

            // assert
            _sut.ActiveOffers.Count.Should().Be(2); // two offers in total
            _sut.ActiveOffers.Where(o => o.Owner.UserId == offerOwnerId).Count().Should().Be(1); // only 1 offer by the owner who submitted two offer
            _sut.ActiveOffers.Where(o => o.Owner.UserId == offerOwnerId).First().MonetaryValue.Value.Should().Be(3M); // last offer overides the previous one
        }

        [Fact]
        public void accept_existing_offer()
        {
            // arrange
            var offerId = Guid.NewGuid();
            Either<Error, ActiveOffer> offer1 =
                from owner in Owner.Create(Guid.NewGuid())
                from value in MonetaryValue.Create(1M, "eur")
                select
                    new ActiveOffer(offerId, owner, value, DateTimeOffset.UtcNow);
            Either<Error, ActiveOffer> offer2 =
                from owner in Owner.Create(Guid.NewGuid())
                from value in MonetaryValue.Create(2M, "eur")
                select
                   new ActiveOffer(Guid.NewGuid(), owner, value, DateTimeOffset.UtcNow);

            Either<Error, Unit> addFirstOfferAction = offer1
                .Bind(offer1 => _sut.ReceiveOffer(offer1));
            Either<Error, Unit> addSecondOfferAction = offer2
                .Bind(offer2 => _sut.ReceiveOffer(offer2));

            // act
            Either<Error, ClosedListing> eitherClosedListing = _sut.AcceptOffer(offerId, DateTimeOffset.Now);

            // assert
            eitherClosedListing
                .Right(l => l.AcceptedOffer.Id.Should().Be(offerId))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> ArgumentsForAcceptOffer => new List<object[]>
        {
            new object[] { default, DateTimeOffset.UtcNow },
            new object[] { Guid.NewGuid(), default }
        };

        [Theory]
        [MemberData(nameof(ArgumentsForAcceptOffer))]
        public void reject_to_accept_offer_if_arguments_are_not_valid(Guid offerId, DateTimeOffset date)
        {
            Either<Error, ClosedListing> action = _sut.AcceptOffer(offerId, date);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Fact]
        public void not_accept_non_existing_offers()
        {
            // arrange
            Either<Error, ActiveOffer> offer1 =
                from owner in Owner.Create(Guid.NewGuid())
                from value in MonetaryValue.Create(1M, "eur")
                select
                    new ActiveOffer(Guid.NewGuid(), owner, value, DateTimeOffset.UtcNow);

            Either<Error, ActiveOffer> offer2 =
                from owner in Owner.Create(Guid.NewGuid())
                from value in MonetaryValue.Create(2M, "eur")
                select
                   new ActiveOffer(Guid.NewGuid(), owner, value, DateTimeOffset.UtcNow);

            Either<Error, Unit> addFirstOfferAction = offer1
                .Bind(offer1 => _sut.ReceiveOffer(offer1));

            Either<Error, Unit> addSecondOfferAction = offer2
                .Bind(offer2 => _sut.ReceiveOffer(offer2));

            // act
            Either<Error, ClosedListing> eitherClosedListing = _sut.AcceptOffer(Guid.NewGuid(), DateTimeOffset.Now);

            // assert
            eitherClosedListing
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Message.Should().Be("offer not found"));
        }

        [Fact]
        public void accept_leads()
        {
            // arrange
            Either<Error, Lead> eitherLead = Lead.Create(Guid.NewGuid(), DateTimeOffset.Now);

            // act & assert
            eitherLead
                .Bind(lead => _sut.AddLead(lead))
                .Right(_ => _sut.Leads.Count.Should().Be(1))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void not_accept_null_leads()
        {
            // act
            Either<Error, Unit> addNullLeadAction = _sut.AddLead(null);

            // assert
            addNullLeadAction
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("invalid lead");
                    _sut.Leads.Count.Should().Be(0);
                });
        }

        [Fact]
        public void not_accept_leads_from_the_listing_owner()
        {
            // arrange
            Either<Error, Lead> eitherLeadFromListingOwner = Lead.Create(_sut.Owner.UserId, DateTimeOffset.Now);

            // act
            Either<Error, Unit> addLeadAction = eitherLeadFromListingOwner
                .Bind(lead => _sut.AddLead(lead));

            // assert
            addLeadAction
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("cannot accept leads from the listing owner");
                    _sut.Leads.Count.Should().Be(0);
                });
        }

        [Fact]
        public void not_accept_leads_from_the_same_owner_more_than_once()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            Either<Error, Lead> eitherFirstLead = Lead.Create(ownerId, DateTimeOffset.Now.AddDays(-1));
            Either<Error, Lead> eitherSecondLead = Lead.Create(ownerId, DateTimeOffset.Now);

            // act
            Either<Error, Unit> addFirstLeadAction = eitherFirstLead
                .Bind(lead => _sut.AddLead(lead));
            Either<Error, Unit> addSecondLead = eitherSecondLead
                .Bind(lead => _sut.AddLead(lead));

            // Assert
            eitherFirstLead.IsRight.Should().BeTrue(); // first add was sucessfull
            addSecondLead
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("lead from this user already exists");
                    _sut.Leads.Count.Should().Be(1);
                });
        }

        [Fact]
        public void be_markable_as_favorite()
        {
            // arrange
            Either<Error, FavoriteMark> eitherFavorite = FavoriteMark.Create(Guid.NewGuid(), DateTimeOffset.UtcNow);

            // act
            Either<Error, Unit> action = eitherFavorite.Bind(favorite => _sut.MarkAsFavorite(favorite));

            //assert
            action
                .Right(_ => _sut.Favorites.Count.Should().Be(1))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void not_accept_null_favorites()
        {
            _sut
                .MarkAsFavorite(null)
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("invalid favorite");
                    _sut.Favorites.Count.Should().Be(0);
                });
        }

        [Fact]
        public void not_be_markable_as_favorite_by_listing_owner()
        {
            FavoriteMark
                .Create(_sut.Owner.UserId, DateTimeOffset.UtcNow)
                .Bind(favorite => _sut.MarkAsFavorite(favorite))
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("cannot accept favorites from the listing owner");
                    _sut.Favorites.Count.Should().Be(0);
                });
        }

        [Fact]
        public void not_be_markable_as_favorite_by_same_owner_more_than_once()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            Either<Error, FavoriteMark> firstFavorite = FavoriteMark.Create(ownerId, DateTimeOffset.UtcNow);
            Either<Error, FavoriteMark> secondFavorite = FavoriteMark.Create(ownerId, DateTimeOffset.UtcNow);

            // Act
            Either<Error, Unit> addFirst = firstFavorite
                .Bind(favorite => _sut.MarkAsFavorite(favorite));
            Either<Error, Unit> addSecond = secondFavorite
                .Bind(favorite => _sut.MarkAsFavorite(favorite));

            // Assert
            addFirst.IsRight.Should().BeTrue();
            addSecond
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("favorite from this user already exists");
                    _sut.Favorites.Count.Should().Be(1);
                });
        }

        [Fact]
        public void have_the_option_to_remove_previously_added_favorite_mark()
        {
            // Arrange
            var userId = Guid.NewGuid();
            Either<Error, Owner> favoriteOwner = Owner.Create(userId);
            Either<Error, FavoriteMark> favoriteMark = FavoriteMark.Create(userId, DateTimeOffset.UtcNow);
            favoriteMark
                .Bind(favorite => _sut.MarkAsFavorite(favorite));

            // Act
            Either<Error, Unit> removeAction = favoriteOwner
                .Bind(owner => _sut.RemoveFavorite(owner));

            // Assert
            removeAction
                .Right(_ => _sut.Favorites.Count.Should().Be(0))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void not_accept_invalid_owners_for_favorite_mark_removal()
        {
            Either<Error, Unit> removeAction = _sut.RemoveFavorite(null);

            removeAction
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("favoredBy");
                    error.Should().BeOfType<Error.Invalid>();
                });
        }

        [Fact]
        public void not_accept_non_existing_owners_for_favorite_mark_removal()
        {
            // Arrange
            Either<Error, Owner> favoriteOwner = Owner.Create(Guid.NewGuid());
            Either<Error, FavoriteMark> favoriteMark = FavoriteMark.Create(Guid.NewGuid(), DateTimeOffset.UtcNow);
            favoriteMark
                .Bind(favorite => _sut.MarkAsFavorite(favorite));

            // Act
            Either<Error, Unit> removeAction = favoriteOwner
                .Bind(owner => _sut.RemoveFavorite(owner));

            // Assert
            removeAction
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("favoredBy");
                    error.Should().BeOfType<Error.NotFound>();
                    _sut.Favorites.Count.Should().Be(1);
                });
        }

        [Fact]
        public void allow_to_mark_received_offers_as_seen()
        {
            // arrange
            var id1 = Guid.NewGuid();
            Either<Error, ActiveOffer> offer1 =
                from owner in Owner.Create(Guid.NewGuid())
                from value in MonetaryValue.Create(1M, "eur")
                select
                    new ActiveOffer(id1, owner, value, DateTimeOffset.UtcNow);

            var id2 = Guid.NewGuid();
            Either<Error, ActiveOffer> offer2 =
                from owner in Owner.Create(Guid.NewGuid())
                from value in MonetaryValue.Create(2M, "eur")
                select
                   new ActiveOffer(id2, owner, value, DateTimeOffset.UtcNow);

            Either<Error, SeenDate> seenDate = SeenDate.Create(DateTimeOffset.Now);

            offer1.Bind(offer1 => _sut.ReceiveOffer(offer1));
            offer2.Bind(offer2 => _sut.ReceiveOffer(offer2));

            // act
            Either<Error, Unit> action = bind(
                seenDate,
                (seenDate) => _sut.MarkOfferAsSeen(id1, seenDate));

            // assert
            action
                .Right(_ =>
                {
                    _sut.ActiveOffers.First(o => o.Id == id1).SeenDate.IsSome.Should().BeTrue();  // first offer has been marked as seen
                    _sut.ActiveOffers.First(o => o.Id == id2).SeenDate.IsSome.Should().BeFalse();
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void mark_only_existing_received_offers_as_seen()
        {
            // arrange
            Either<Error, SeenDate> seenDate = SeenDate.Create(DateTimeOffset.Now);

            // act
            Either<Error, Unit> action = bind(
                seenDate,
                (seenDate) => _sut.MarkOfferAsSeen(Guid.NewGuid(), seenDate));

            // Assert
            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("offerId");
                    error.Should().BeOfType<Error.NotFound>();
                });
        }


        public static IEnumerable<object[]> ArgumentsForMarkOfferAsSeen => new List<object[]>
        {
            new object[] { default, TestValueObjectFactory.CreateSeenDate(DateTimeOffset.UtcNow) },
            new object[] { Guid.NewGuid(), null }
        };

        [Theory]
        [MemberData(nameof(ArgumentsForMarkOfferAsSeen))]
        public void reject_to_mark_offer_as_seen_if_arguments_are_not_valid(Guid offerId, SeenDate seenDate)
        {
            Either<Error, Unit> action = _sut.MarkOfferAsSeen(offerId, seenDate);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }
    }
}
