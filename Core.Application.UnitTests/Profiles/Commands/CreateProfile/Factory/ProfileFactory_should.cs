using Common.Dates;
using Common.Helpers;
using Core.Application.Profiles.Commands.CreateProfile.Factory;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.CreateProfile.Factory
{
    public class ProfileFactory_should
    {
        private static Guid _userId = Guid.NewGuid();
        private static Email _email = TestValueObjectFactory.CreateEmail("aaa@bbb.ccc");
        private static ContactDetails _contactDetails = TestValueObjectFactory.CreateContactDetails("mike", "tyson", "asd", "+333 111 22222");
        private static LocationDetails _locationDetails = TestValueObjectFactory.CreateLocationDetails("LT", "staaaat", "vilnius", "aaa1", "some random place 12");
        private static GeographicLocation _geographicLocation = TestValueObjectFactory.CreateGeographicLocation(10D, 10D);
        private static UserPreferences _userPreferences = TestValueObjectFactory.CreateUserPreferences(DistanceMeasurementUnit.Kilometer.Symbol, MassMeasurementUnit.Kilogram.Symbol, CurrencyCode.EUR.Value);
        private AutoMocker _mocker;
        private ProfileFactory _sut;

        public ProfileFactory_should()
        {
            _mocker = new AutoMocker();
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<ProfileFactory>();
        }

        [Fact]
        public void return_EitherRight_with_Message_on_success()
        {
            // act
            Either<Error, ActiveProfile> eitherProfile = _sut.CreateActive(
                _userId,
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences);

            //assert
            eitherProfile
                .Right(offer => offer.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { default, _email, _contactDetails, _locationDetails, _geographicLocation, _userPreferences, "userId" },
            new object[] { _userId, null, _contactDetails, _locationDetails, _geographicLocation, _userPreferences, "email" },
            new object[] { _userId, _email, null, _locationDetails, _geographicLocation, _userPreferences, "contactDetails" },
            new object[] { _userId, _email, _contactDetails, null, _geographicLocation, _userPreferences, "locationDetails" },
            new object[] { _userId, _email, _contactDetails, _locationDetails, null, _userPreferences, "geographicLocation" },
            new object[] { _userId, _email, _contactDetails, _locationDetails, _geographicLocation, null, "userPreferences" }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void return_EitherLeft_with_proper_error_when_arguments_are_invalid(
            Guid userId,
            Email email,
            ContactDetails contactDetails, 
            LocationDetails locationDetails, 
            GeographicLocation geographicLocation,
            UserPreferences userPreferences, 
            string errorMessage)
        {
            // act
            Either<Error, ActiveProfile> eitherProfile = _sut.CreateActive(
                userId,
                email,
                contactDetails,
                locationDetails,
                geographicLocation,
                userPreferences);

            //assert
            eitherProfile
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be(errorMessage);
                });
        }
    }
}
