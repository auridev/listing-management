﻿using Core.Domain.ValueObjects;
using Core.Domain.Profiles;
using Test.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using Common.Helpers;

namespace BusinessLine.Core.Domain.UnitTests.Profiles
{
    public abstract class Profile_should
    {
        protected static readonly Email _email = 
            TestValueObjectFactory.CreateEmail("aaa@bbbb.com");
        protected static readonly ContactDetails _contactDetails = 
            TestValueObjectFactory.CreateContactDetails("mike", "tyson", "asd", "+333 111 22222");
        protected static readonly LocationDetails _locationDetails = 
            TestValueObjectFactory.CreateLocationDetails("LT", "staaaat", "vilnius", "aaa1", "some random place 12");
        protected static readonly GeographicLocation _geographicLocation = 
            TestValueObjectFactory.CreateGeographicLocation(10D, 10D);
        protected static readonly UserPreferences _userPreferences = 
            TestValueObjectFactory.CreateUserPreferences(DistanceMeasurementUnit.Kilometer.Symbol, MassMeasurementUnit.Kilogram.Symbol, CurrencyCode.EUR.Value);
        protected static readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        private readonly Profile _sut;

        public Profile_should()
        {
            _sut = new ProfileTestFake(Guid.NewGuid(),
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);
        }

        [Fact]
        public void have_an_Id_property()
        {
            _sut.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_UserId_property()
        {
            _sut.UserId.Should().NotBeEmpty();
        }

        [Fact]
        public void have_an_Email_property()
        {
            _sut.Email.Should().Be(_email);
        }

        [Fact]
        public void have_a_ContactDetails_property()
        {
            _sut.ContactDetails.Should().Be(_contactDetails);
        }

        [Fact]
        public void have_a_LocationDetails_property()
        {
            _sut.LocationDetails.Should().Be(_locationDetails);
        }

        [Fact]
        public void have_a_GeographicLocation_property()
        {
            _sut.GeographicLocation.Should().Be(_geographicLocation);
        }

        [Fact]
        public void have_a_UserPreferences_property()
        {
            _sut.UserPreferences.Should().Be(_userPreferences);
        }

        [Fact]
        public void have_a_CreatedDate_property()
        {
            _sut.CreatedDate.Should().BeCloseTo(_createdDate);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { Guid.Empty, Guid.NewGuid(), _email, _contactDetails, _locationDetails, _geographicLocation, _userPreferences, _createdDate },
            new object[] { Guid.NewGuid(), Guid.Empty, _email, _contactDetails, _locationDetails, _geographicLocation, _userPreferences, _createdDate },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), null, _contactDetails, _locationDetails, _geographicLocation, _userPreferences, _createdDate },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), _email, null, _locationDetails, _geographicLocation, _userPreferences, _createdDate },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), _email, _contactDetails, null, _geographicLocation, _userPreferences, _createdDate },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), _email, _contactDetails, _locationDetails, null, _userPreferences, _createdDate },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), _email, _contactDetails, _locationDetails, _geographicLocation, null, _createdDate },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), _email, _contactDetails, _locationDetails, _geographicLocation, _userPreferences, default},
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void thrown_an_exception_during_creation_if_arguments_are_not_valid(Guid id,
            Guid userId,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences,
            DateTimeOffset createdDate)
        {
            Action createAction = () => new ProfileTestFake(id,
                userId,
                email,
                contactDetails,
                locationDetails,
                geographicLocation,
                userPreferences,
                createdDate);

            createAction.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new ProfileTestFake(id,
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);
            var second = new ProfileTestFake(id,
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);

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
            var first = (object)new ProfileTestFake(id,
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);
            var second = (object)new ProfileTestFake(id,
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);

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
            var first = new ProfileTestFake(id,
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);
            var second = new ProfileTestFake(id,
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);

            // act
            var equals = (first == second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_ids_dont_match()
        {
            // arrange
            var first = new ProfileTestFake(Guid.NewGuid(),
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);
            var second = new ProfileTestFake(Guid.NewGuid(),
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);

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
            var first = new ProfileTestFake(id,
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);
            var second = new ProfileTestFake(id,
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);

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
