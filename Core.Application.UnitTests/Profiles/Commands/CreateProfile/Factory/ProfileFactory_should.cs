using Core.Application.Profiles.Commands.CreateProfile.Factory;
using Core.Domain.Common;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.CreateProfile.Factory
{
    public class ProfileFactory_should
    {
        private readonly Email _email;
        private readonly ContactDetails _contactDetails;
        private readonly LocationDetails _locationDetails;
        private readonly GeographicLocation _geographicLocation;
        private readonly UserPreferences _userPreferences;

        public ProfileFactory_should()
        {
            _email = Email.Create("aaa@bbbb.com");
            _contactDetails = ContactDetails.Create(PersonName.Create("mike", "tyson"),
                Company.Create("asd"),
                Phone.Create("+333 111 22222"));
            _locationDetails = LocationDetails.Create(Alpha2Code.Create("LT"),
                State.Create("staaaat"),
                City.Create("vilnius"),
                PostCode.Create("aaa1"),
                Address.Create("some random place 12"));
            _geographicLocation = GeographicLocation.Create(10D, 10D);
            _userPreferences = UserPreferences.Create(
                DistanceMeasurementUnit.Kilometer,
                MassMeasurementUnit.Kilogram,
                CurrencyCode.Create("eur"));
        }

        [Fact]
        public void create_an_active_profile()
        {
            var factory = new ProfileFactory();

            var activeProfile = factory.CreateActive(Guid.NewGuid(),
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                DateTimeOffset.UtcNow);

            activeProfile.Should().NotBeNull();
        }
    }
}
