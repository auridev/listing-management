﻿using Core.Domain.Common;
using Core.Domain.Listings;
using Core.UnitTests.Mocks;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public abstract class Listing_should
    {
        protected static readonly Owner _owner = Owner.Create(Guid.NewGuid());
        protected static readonly ListingDetails _listingDetails = ListingDetails.Create(Title.Create("my title"),
                MaterialType.Plastic,
                Weight.Create(2.5F, MassMeasurementUnit.Kilogram),
                Description.Create("somethin nice to sell"));
        protected static readonly ContactDetails _contactDetails = ContactDetails.Create(PersonName.Create("john", "doe"),
                Company.Create("gariunai"),
                Phone.Create("+333 111 22222"));
        protected static readonly LocationDetails _locationDetails = LocationDetails.Create(
            Alpha2Code.Create("LT"),
            State.Create("staaaat"),
            City.Create("polis"),
            PostCode.Create("aaa1"),
            Address.Create("some random place 12"));
        protected static readonly GeographicLocation _geographicLocation = GeographicLocation.Create(20D, 30D);

        private readonly Listing _sut;

        public Listing_should()
        {
            _sut = new ListingTestFake(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "system under test");
        }

        [Fact]
        public void have_an_Id_property()
        {
            _sut.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void have_an_Owner_property()
        {
            _sut.Owner.UserId.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_ListingDetails_property()
        {
            _sut.ListingDetails.Should().NotBeNull();
        }

        [Fact]
        public void have_a_ContactDetails_property()
        {
            _sut.ContactDetails.Should().NotBeNull();
        }

        [Fact]
        public void have_a_LocationDetails_property()
        {
            _sut.LocationDetails.Should().NotBeNull();
        }

        [Fact]
        public void have_a_GeographicLocation_property()
        {
            _sut.GeographicLocation.Should().NotBeNull();
        }

        [Fact]
        public void have_a_DaysUntilExpiration_constant()
        {
            Listing.DaysUntilExpiration.Should().Be(90);
        }

        public static IEnumerable<object[]> InvalidArgumentsForBaseListing => new List<object[]>
        {
            new object[] { Guid.Empty, _owner, _listingDetails, _contactDetails, _locationDetails, _geographicLocation },
            new object[] { Guid.NewGuid(), null, _listingDetails, _contactDetails, _locationDetails, _geographicLocation },
            new object[] { Guid.NewGuid(), _owner, null, _contactDetails, _locationDetails, _geographicLocation },
            new object[] { Guid.NewGuid(), _owner, _listingDetails, null, _locationDetails, _geographicLocation },
            new object[] { Guid.NewGuid(), _owner, _listingDetails, _contactDetails, null, _geographicLocation },
            new object[] { Guid.NewGuid(), _owner, _listingDetails, _contactDetails, _locationDetails, null }
        };

        [Theory]
        [MemberData(nameof(InvalidArgumentsForBaseListing))]
        public void thrown_an_exception_during_creation_if_arguments_are_not_valid(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation)
        {
            Action createAction = () => new ListingTestFake(id,
                owner,
                listingDetails,
                contactDetails,
                locationDetails,
                geographicLocation,
                "aaa");

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new ListingTestFake(id,
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "first");
            var second = new ListingTestFake(id,
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "second");

            // act
            var equals = first.Equals(second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = (object)new ListingTestFake(id,
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "first");
            var second = (object)new ListingTestFake(id,
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "second");

            // act
            var equals = first.Equals(second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new ListingTestFake(id,
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "first");
            var second = new ListingTestFake(id,
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "second");

            // act
            var equals = (first == second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_ids_dont_match()
        {
            // arrange
            var first = new ListingTestFake(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "first");
            var second = new ListingTestFake(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "second");

            // act
            var nonEquals = (first != second);

            // assert
            nonEquals.Should().BeTrue();
        }

        [Fact]
        public void have_the_same_hashcode_as_an_equal_listing()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new ListingTestFake(id,
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "aaaaaaaaaa");
            var second = new ListingTestFake(id,
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                "bbbbbbbbbbb");

            // act
            var equals = (first == second);
            var firstCode = first.GetHashCode();
            var secondCode = second.GetHashCode();

            // assert
            equals.Should().BeTrue();
            firstCode.Should().Be(secondCode);
        }
    }
}