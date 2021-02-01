using Core.Domain.Listings;
using Core.Domain.Offers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class ClosedListing_should : Listing_should
    {
        private static readonly AcceptedOffer _acceptedOffer = new AcceptedOffer(
            Guid.NewGuid(),
            TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
            TestValueObjectFactory.CreateMonetaryValue(12.5M, "eur"),
            DateTimeOffset.UtcNow);

        private static readonly List<ClosedOffer> _closedOffers = new List<ClosedOffer>();

        private readonly ClosedListing _sut = new ClosedListing(
            Guid.NewGuid(),
            _owner,
            _listingDetails,
            _contactDetails,
            _locationDetails,
            _geographicLocation,
            _createdDate,
            _acceptedOffer,
            _closedOffers);


        public static IEnumerable<object[]> InvalidArgumentsForClosedListing => new List<object[]>
        {
            new object[] { _acceptedOffer,  null },
            new object[] { null, _closedOffers }
        };

        [Theory]
        [MemberData(nameof(InvalidArgumentsForClosedListing))]
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid(AcceptedOffer acceptedOffer, List<ClosedOffer> closedOffers)
        {
            Action createAction = () => new ClosedListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                acceptedOffer,
                closedOffers);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void have_an_AcceptedOffer_property()
        {
            _sut.AcceptedOffer.Should().NotBeNull();
        }

        [Fact]
        public void have_ClosedOffers_property()
        {
            _sut.ClosedOffers.Should().NotBeNull();
        }
    }
}
