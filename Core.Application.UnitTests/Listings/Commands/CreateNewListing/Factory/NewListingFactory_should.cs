using Common.Helpers;
using Core.Application.Listings.Commands.CreateNewListing.Factory;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.CreateNewListing.Factory
{
    public class NewListingFactory_should
    {
        private static Owner _owner = TestValueObjectFactory.CreateOwner(Guid.NewGuid());
        private static ListingDetails _listingDetails = DummyData.ListingDetails;
        private static ContactDetails _contactDetails = DummyData.ContactDetails;
        private static LocationDetails _locationDetails = DummyData.LocationDetails;
        private static GeographicLocation _geographicLocation = DummyData.GeographicLocation;
        private static DateTimeOffset _createdDate = DateTimeOffset.UtcNow;
        private NewListingFactory _sut = new NewListingFactory();

        [Fact]
        public void return_EitherRight_with_NewListing_on_success()
        {
            // act
            Either<Error, NewListing> eitherNewListing = _sut.Create(
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate);

            //assert
            eitherNewListing
                .Right(listing => listing.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null, _listingDetails, _contactDetails, _locationDetails, _geographicLocation, _createdDate, "owner" },
            new object[] { _owner, null, _contactDetails, _locationDetails, _geographicLocation, _createdDate, "listingDetails" },
            new object[] { _owner, _listingDetails, null, _locationDetails, _geographicLocation, _createdDate, "contactDetails" },
            new object[] { _owner, _listingDetails, _contactDetails, null, _geographicLocation, _createdDate, "locationDetails" },
            new object[] { _owner, _listingDetails, _contactDetails, _locationDetails, null, _createdDate, "geographicLocation" },
            new object[] { _owner, _listingDetails, _contactDetails, _locationDetails, _geographicLocation, default, "createdDate" }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void return_EitherLeft_with_proper_error_when_arguments_are_invalid(
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate,
            string errorMessage)
        {
            // act
            Either<Error, NewListing> eitherNewListing = _sut.Create(
                owner,
                listingDetails,
                contactDetails,
                locationDetails,
                geographicLocation,
                createdDate);

            //assert
            eitherNewListing
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be(errorMessage);
                });
        }
    }
}
