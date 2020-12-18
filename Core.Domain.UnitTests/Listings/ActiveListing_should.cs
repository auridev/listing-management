using Core.Domain.Common;
using Core.Domain.Listings;
using Core.Domain.Offers;
using FluentAssertions;
using LanguageExt;
using System;
using System.Linq;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class ActiveListing_should : Listing_should
    {
        private readonly ActiveListing _sut;

        public ActiveListing_should()
        {
            _sut = new ActiveListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow.AddDays(90));
        }

        [Fact]
        public void have_an_ExpirationDate_property()
        {
            _sut.ExpirationDate.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(90));
        }

        [Fact]
        public void have_an_Offers_property()
        {
            _sut.Offers.Should().NotBeNull();
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
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid()
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
            // act
            PassiveListing passiveListing = _sut.Deactivate(TrimmedString.Create("wrong number"), DateTimeOffset.UtcNow);

            // assert
            passiveListing.Should().NotBeNull();
            passiveListing.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            passiveListing.Reason.Value.Should().Be("wrong number");
        }

        [Fact]
        public void be_able_to_receive_offers()
        {
            var offer = new ReceivedOffer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, 
                    CurrencyCode.Create("usd")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));

            _sut.ReceiveOffer(offer);

            _sut.Offers.Count.Should().Be(1);
        }

        [Fact]
        public void not_receive_null_offers()
        {
            Action action = () => _sut.ReceiveOffer(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ignore_offers_from_the_owner_of_the_listing()
        {
            // arrange
            Owner sameOwner = Owner.Create(Guid.NewGuid());
            var listing = new ActiveListing(Guid.NewGuid(),
                sameOwner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow.AddDays(24));
            var offer = new ReceivedOffer(Guid.NewGuid(),
                sameOwner,
                MonetaryValue.Create(34.89M, CurrencyCode.Create("usd")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));

            // act
            listing.ReceiveOffer(offer);

            // assert
            listing.Offers.Count.Should().Be(0);
        }

        [Fact]
        public void replace_previous_offer_of_an_owner_if_another_one_is_received()
        {
            // arrange
            Owner listingOwner = Owner.Create(Guid.NewGuid());
            Owner offerOwner = Owner.Create(Guid.NewGuid());
            var listing = new ActiveListing(Guid.NewGuid(),
                listingOwner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow.AddDays(24));
            var dymmyOffer = new ReceivedOffer(Guid.NewGuid(), // just an offer from some owner to increase the count
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer1 = new ReceivedOffer(Guid.NewGuid(), // two offers by the same owner
                offerOwner,
                MonetaryValue.Create(6M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer2 = new ReceivedOffer(Guid.NewGuid(),
                offerOwner,
                MonetaryValue.Create(3M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));

            // act
            _sut.ReceiveOffer(dymmyOffer);
            _sut.ReceiveOffer(offer1);
            _sut.ReceiveOffer(offer2);

            // assert
            _sut.Offers.Count.Should().Be(2); // two offers in total
            _sut.Offers.Where(o => o.Owner == offerOwner).Count().Should().Be(1); // only 1 offer by the owner who submitted two offer
            _sut.Offers.Where(o => o.Owner == offerOwner).First().MonetaryValue.Value.Should().Be(3M); // last offer overides the previous one
        }

        [Fact]
        public void accept_existing_offer()
        {
            // arrange
            var offerId = Guid.NewGuid();

            var offer1 = new ReceivedOffer(offerId,
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer2 = new ReceivedOffer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            _sut.ReceiveOffer(offer1);
            _sut.ReceiveOffer(offer2);

            // act
            Option<ClosedListing> optionalClosedListing = _sut.AcceptOffer(offerId, DateTimeOffset.Now);

            // assert
            optionalClosedListing.IsSome.Should().BeTrue();
            optionalClosedListing.IfSome(l => l.AcceptedOffer.Id.Should().Be(offer1.Id));
        }

        [Fact]
        public void not_accept_non_existing_offers()
        {
            // arrange
            var offer1 = new ReceivedOffer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer2 = new ReceivedOffer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            _sut.ReceiveOffer(offer1);
            _sut.ReceiveOffer(offer2);

            // act
            Option<ClosedListing> optionalClosedListing = _sut.AcceptOffer(Guid.NewGuid(), DateTimeOffset.Now);

            // assert
            optionalClosedListing.IsSome.Should().BeFalse();
        }

        [Fact]
        public void mark_other_offers_as_rejected()
        {
            // arrange
            var offerId = Guid.NewGuid();

            var offer1 = new ReceivedOffer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer2 = new ReceivedOffer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(2M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer3 = new ReceivedOffer(offerId,
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(3M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            _sut.ReceiveOffer(offer1);
            _sut.ReceiveOffer(offer2);
            _sut.ReceiveOffer(offer3);

            // act
            Option<ClosedListing> optionalClosedListing = _sut.AcceptOffer(offerId, DateTimeOffset.Now);

            // assert
            optionalClosedListing.IfSome(l => l.RejectedOffers[0].Id.Should().Be(offer1.Id));
            optionalClosedListing.IfSome(l => l.RejectedOffers[1].Id.Should().Be(offer2.Id));
        }

        [Fact]
        public void have_a_Leads_property()
        {
            _sut.Leads.Should().NotBeNull();
        }

        [Fact]
        public void accept_leads()
        {
            var lead = Lead.Create(Owner.Create(Guid.NewGuid()), DateTimeOffset.Now);

            _sut.AddLead(lead);

            _sut.Leads.Count.Should().Be(1);
        }

        [Fact]
        public void not_accept_null_leads()
        {
            Action action = () => _sut.AddLead(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void not_accept_leads_from_the_listing_owner()
        {
            var leadFromListingOwner = Lead.Create(_sut.Owner, DateTimeOffset.Now);

            _sut.AddLead(leadFromListingOwner);

            _sut.Leads.Count.Should().Be(0);
        }

        [Fact]
        public void not_accept_leads_from_the_same_owner_more_than_once()
        {
            // Arrange
            var owner = Owner.Create(Guid.NewGuid());
            var firstLead = Lead.Create(owner, DateTimeOffset.Now.AddDays(-1));
            var secondLead = Lead.Create(owner, DateTimeOffset.Now);

            // Act
            _sut.AddLead(firstLead);
            _sut.AddLead(secondLead);

            // Assert
            _sut.Leads.Count.Should().Be(1);
        }

        [Fact]
        public void have_Favorites_property()
        {
            _sut.Favorites.Should().NotBeNull();
        }

        [Fact]
        public void be_markable_as_favorite()
        {
            var favorite = FavoriteMark.Create(Owner.Create(Guid.NewGuid()), DateTimeOffset.UtcNow);

            _sut.MarkAsFavorite(favorite);

            _sut.Favorites.Count.Should().Be(1);
        }

        [Fact]
        public void throw_exception_when_favorite_is_null()
        {
            Action action = () => _sut.MarkAsFavorite(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void not_be_markable_as_favorite_by_listing_owner()
        {
            var favoriteByListingOwner = FavoriteMark.Create(_sut.Owner, DateTimeOffset.UtcNow);

            _sut.MarkAsFavorite(favoriteByListingOwner);

            _sut.Favorites.Count.Should().Be(0);
        }

        [Fact]
        public void not_be_markable_as_favorite_by_same_owner_more_than_once()
        {
            // Arrange
            var owner = Owner.Create(Guid.NewGuid());
            var first = FavoriteMark.Create(owner, DateTimeOffset.UtcNow);
            var second = FavoriteMark.Create(owner, DateTimeOffset.UtcNow);

            // Act
            _sut.MarkAsFavorite(first);
            _sut.MarkAsFavorite(second);

            // Assert
            _sut.Favorites.Count.Should().Be(1);
        }

        [Fact]
        public void have_the_option_to_remove_previously_added_favorite_mark()
        {
            // Arrange
            var favoredBy = Owner.Create(Guid.NewGuid());
            var favorite = FavoriteMark.Create(favoredBy, DateTimeOffset.UtcNow);
            _sut.MarkAsFavorite(favorite);

            // Act
            _sut.RemoveFavorite(favoredBy);

            // Assert
            _sut.Favorites.Count.Should().Be(0);
        }

        [Fact]
        public void not_acceps_invalid_owners_for_favorite_mark_removal()
        {
            Action action = () => _sut.RemoveFavorite(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void allow_to_mark_received_offers_as_seen()
        {
            // arrange
            var offerId1 = Guid.NewGuid();
            var offer1 = new ReceivedOffer(offerId1,
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                Option<SeenDate>.None);
            var offerId2 = Guid.NewGuid();
            var offer2 = new ReceivedOffer(offerId2,
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(2M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                Option<SeenDate>.None);

            _sut.ReceiveOffer(offer1);
            _sut.ReceiveOffer(offer2);

            // act
            _sut.MarkOfferAsSeen(offerId1, SeenDate.Create(DateTimeOffset.Now));

            // assert
            _sut.Offers.First(o => o == offer1).SeenDate.IsSome.Should().BeTrue();
            _sut.Offers.First(o => o == offer2).SeenDate.IsSome.Should().BeFalse();
        }

        [Fact]
        public void only_mark_existing_received_offers_as_seen()
        {
            // arrange
            var offerId1 = Guid.NewGuid();
            var offer1 = new ReceivedOffer(offerId1,
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                Option<SeenDate>.None);

            _sut.ReceiveOffer(offer1);

            // act
            _sut.MarkOfferAsSeen(Guid.NewGuid(), SeenDate.Create(DateTimeOffset.Now));

            // assert
            _sut.Offers.First(o => o == offer1).SeenDate.IsSome.Should().BeFalse();
        }
    }
}
