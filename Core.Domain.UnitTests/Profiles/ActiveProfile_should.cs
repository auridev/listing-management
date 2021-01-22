using Common.Helpers;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Profiles
{
    public class ActiveProfile_should : Profile_should
    {
        private readonly ActiveProfile _sut;

        public ActiveProfile_should()
        {
            _sut = new ActiveProfile(Guid.NewGuid(),
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate);
        }

        [Fact]
        public void be_able_to_update_details()
        {
            // arrange
            var newContactDetails =
                TestValueObjectFactory.CreateContactDetails("keanu", "reaves", "matrix", "+555 111 22222");
            var newLocationDetails =
                TestValueObjectFactory.CreateLocationDetails("us", "California", "LA", "aaa1", "some random place 12");
            var newGeographicLocation =
                TestValueObjectFactory.CreateGeographicLocation(10D, 10D);
            var newUserPreferences =
                TestValueObjectFactory.CreateUserPreferences(DistanceMeasurementUnit.Mile.Symbol, MassMeasurementUnit.Pound.Symbol, "uds");

            // act
            Either<Error, Unit> action =
                _sut.UpdateDetails(newContactDetails, newLocationDetails, newGeographicLocation, newUserPreferences);

            // assert
            action
                .Right(_ =>
                {
                    _sut.ContactDetails.Should().Be(newContactDetails);
                    _sut.LocationDetails.Should().Be(newLocationDetails);
                    _sut.GeographicLocation.Should().Be(newGeographicLocation);
                    _sut.UserPreferences.Should().Be(newUserPreferences);
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> ArgumentsForUpdateDetails => new List<object[]>
        {
            new object[] { null, _locationDetails, _geographicLocation, _userPreferences },
            new object[] { _contactDetails, null, _geographicLocation, _userPreferences },
            new object[] { _contactDetails, _locationDetails, null, _userPreferences },
            new object[] { _contactDetails, _locationDetails, _geographicLocation, null }
        };

        [Theory]
        [MemberData(nameof(ArgumentsForUpdateDetails))]
        public void reject_to_update_details_if_arguments_are_not_valid(ContactDetails cd, LocationDetails ld, GeographicLocation gl, UserPreferences up)
        {
            Either<Error, Unit> action = _sut.UpdateDetails(cd, ld, gl, up);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Fact]
        public void be_deactivatable()
        {
            // arrange
            var reason = TestValueObjectFactory.CreateTrimmedString("trial expired");

            // act
            Either<Error, PassiveProfile> action = _sut.Deactivate(DateTimeOffset.UtcNow, reason);

            // assert
            action
               .Right(passiveProfile =>
               {
                   passiveProfile.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
                   passiveProfile.Reason.ToString().Should().Be("trial expired");
               })
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> ArgumentsForDeactivate => new List<object[]>
        {
            new object[] { default, TestValueObjectFactory.CreateTrimmedString("aaaa") },
            new object[] { DateTimeOffset.UtcNow, null }
        };

        [Theory]
        [MemberData(nameof(ArgumentsForDeactivate))]
        public void reject_to_deactivate_if_arguments_are_not_valid(DateTimeOffset date, TrimmedString reason)
        {
            Either<Error, PassiveProfile> action = _sut.Deactivate(date, reason);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Fact]
        public void be_able_to_see_introduction()
        {
            // arrange
            var seenDate = TestValueObjectFactory.CreateSeenDate(DateTimeOffset.UtcNow);

            // act
            Either<Error, Unit> action = _sut.HasSeenIntroduction(seenDate);

            // assert
            action
               .Right(_ =>
               {
                   _sut.IntroductionSeenOn.IsSome.Should().BeTrue();
                   _sut.IntroductionSeenOn.Some(seen => seen.Value.Should().BeCloseTo(DateTimeOffset.UtcNow));
               })
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void reject_to_be_marked_as_seen_introduction_if_seen_date_is_invalid()
        {
            Either<Error, Unit> action = _sut.HasSeenIntroduction(null);

            action
                 .Right(_ => throw InvalidExecutionPath.Exception)
                 .Left(error => error.Should().BeOfType<Error.Invalid>());
        }
    }
}
