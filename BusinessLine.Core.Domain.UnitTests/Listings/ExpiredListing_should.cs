using BusinessLine.Core.Domain.Listings;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class ExpiredListing_should : Listing_should
    {
        [Fact]
        public void have_an_ExpiredOn_property()
        {
            var newListing = new ExpiredListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                DateTimeOffset.UtcNow.AddDays(-1));

            newListing.ExpiredOn.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(-1));
        }

        [Fact(Skip = "this should be in command logic")]
        public void thrown_an_exception_during_creation_if_ExpiredOn_is_in_the_future()
        {
            Action createAction = () => new ExpiredListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                DateTimeOffset.UtcNow.AddDays(1));

            createAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid()
        {
            Action createAction = () => new ExpiredListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                default);

            createAction.Should().Throw<ArgumentNullException>();
        }
    }
}
