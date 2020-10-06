using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class ClosedListing_should : Listing_should
    {
        private static readonly Offer _acceptedOffer = new Offer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(12.5M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));

        [Fact]
        public void have_a_ClosedOn_property()
        {
            var closedListing = new ClosedListing(Guid.NewGuid(), 
                _owner, 
                _listingDetails, 
                _contactDetails,
                _locationDetails, 
                _geographicLocation,
                DateTimeOffset.UtcNow.AddDays(-1), 
                _acceptedOffer);

            closedListing.ClosedOn.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(-1));
        }

        public static IEnumerable<object[]> InvalidArgumentsForClosedListing => new List<object[]>
        {
            new object[] { default, _acceptedOffer },
            new object[] { DateTimeOffset.Now, null }
        };

        [Theory]
        [MemberData(nameof(InvalidArgumentsForClosedListing))]
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid(DateTimeOffset closedOn, Offer acceptedOffer)
        {
            Action createAction = () => new ClosedListing(Guid.NewGuid(), 
                _owner, 
                _listingDetails, 
                _contactDetails,
                _locationDetails, 
                _geographicLocation,
                closedOn,
                acceptedOffer);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void have_an_AcceptedOffer_property()
        {
            var closedListing = new ClosedListing(Guid.NewGuid(), 
                _owner, 
                _listingDetails, 
                _contactDetails,
                _locationDetails, 
                _geographicLocation,
                DateTimeOffset.UtcNow.AddDays(-2), 
                _acceptedOffer);

            closedListing.AcceptedOffer.Should().NotBeNull();
        }
    }
}
