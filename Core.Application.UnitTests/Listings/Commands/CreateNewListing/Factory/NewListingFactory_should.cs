using Core.Application.Listings.Commands.CreateNewListing.Factory;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using Core.Domain.Common;
using Core.Domain.Listings;
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
            var listingDetails = ValueObjectMocks.ListingDetails;
            var contactDetails = ValueObjectMocks.ContactDetails;
            var locationDetail = ValueObjectMocks.LocationDetails;
            var geographicLocation = ValueObjectMocks.GeographicLocation;
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
