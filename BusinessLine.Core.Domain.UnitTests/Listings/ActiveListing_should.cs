using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
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
            var offer = new Offer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, 
                    CurrencyCode.Create("usd")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));

            _sut.ReceiveOffer(offer);

            _sut.Offers.Count.Should().Be(1);
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
                DateTimeOffset.UtcNow.AddDays(24));
            var offer = new Offer(Guid.NewGuid(),
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
                DateTimeOffset.UtcNow.AddDays(24));
            var dymmyOffer = new Offer(Guid.NewGuid(), // just an offer from some owner to increase the count
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer1 = new Offer(Guid.NewGuid(), // two offers by the same owner
                offerOwner,
                MonetaryValue.Create(6M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer2 = new Offer(Guid.NewGuid(),
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
            var offer1 = new Offer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer2 = new Offer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            _sut.ReceiveOffer(offer1);
            _sut.ReceiveOffer(offer2);

            // act
            Option<ClosedListing> optionalClosedListing = _sut.AcceptOffer(offer1, DateTimeOffset.Now);

            // assert
            optionalClosedListing.IsSome.Should().BeTrue();
            optionalClosedListing.IfSome(l => l.AcceptedOffer.Should().Be(offer1));
        }

        [Fact]
        public void not_create_a_closed_listing_if_offer_does_not_exist_in_the_listing()
        {
            // arrange
            var offer1 = new Offer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            var offer2 = new Offer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(1M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));
            _sut.ReceiveOffer(offer1);

            // act
            Option<ClosedListing> optionalClosedListing = _sut.AcceptOffer(offer2, DateTimeOffset.Now);

            // assert
            optionalClosedListing.IsNone.Should().BeTrue();
        }
    }
}
