using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Profiles;
using FluentAssertions;
using System;
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
                SeenDate.CreateNone());
        }

        [Fact]
        public void be_able_to_update_details()
        {
            // arrange
            var newContactDetails = ContactDetails.Create(PersonName.Create("keanu", "reaves"),
                Company.Create("matrix"),
                Phone.Create("+555 111 22222"));
            var newLocationDetails = LocationDetails.Create(Alpha2Code.Create("us"),
                State.Create("California"),
                City.Create("LA"),
                PostCode.Create("aaa1"),
                Address.Create("some random place 12"));
            var newGeographicLocation = GeographicLocation.Create(10D, 10D);
            var newUserPreferences = UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("uds"));

            // act
            _sut.UpdateDetails(newContactDetails, newLocationDetails, newGeographicLocation, newUserPreferences);

            // assert
            _sut.ContactDetails.Should().Be(newContactDetails);
            _sut.LocationDetails.Should().Be(newLocationDetails);
            _sut.GeographicLocation.Should().Be(newGeographicLocation);
            _sut.UserPreferences.Should().Be(newUserPreferences);
        }

        [Fact]
        public void be_deactivatable()
        {
            PassiveProfile passiveProfile = _sut.Deactivate(DateTimeOffset.UtcNow,
                TrimmedString.Create("trial expired"));

            passiveProfile.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            passiveProfile.Reason.ToString().Should().Be("trial expired");
        }

        [Fact]
        public void have_optional_IntroductionSeenOn_property()
        {
            _sut.IntroductionSeenOn.Should().BeOfType(typeof(NotSeen));
        }

        [Fact]
        public void be_able_to_see_introcduction()
        {
            _sut.HasSeenIntroduction(SeenDate.Create(DateTimeOffset.UtcNow));

            _sut.IntroductionSeenOn.Value.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }
    }
}
