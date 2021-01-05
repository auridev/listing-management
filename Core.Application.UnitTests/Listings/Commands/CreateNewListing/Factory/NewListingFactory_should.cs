using Core.Application.Listings.Commands.CreateNewListing.Factory;
using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using Core.UnitTests.Mocks;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.CreateNewListing.Factory
{
    public class NewListingFactory_should
    {
        [Fact]
        public void create_new_listing()
        {
            // arrange
            var owner = Owner.Create(Guid.NewGuid());
            var listingDetails = FakesCollection.ListingDetails;
            var contactDetails = FakesCollection.ContactDetails;
            var locationDetail = FakesCollection.LocationDetails;
            var geographicLocation = FakesCollection.GeographicLocation;
            var createdDate = DateTimeOffset.UtcNow;
            var sut = new NewListingFactory();

            // act
            NewListing newListing = sut.Create(
                owner,
                listingDetails,
                contactDetails,
                locationDetail,
                geographicLocation,
                createdDate);

            //assert
            newListing.Should().NotBeNull();
        }
    }
}
