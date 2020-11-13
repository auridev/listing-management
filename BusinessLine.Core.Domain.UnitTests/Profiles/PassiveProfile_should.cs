using Core.Domain.Common;
using Core.Domain.Profiles;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Profiles
{
    public class PassiveProfile_should : Profile_should
    {
        private readonly PassiveProfile _sut;

        public PassiveProfile_should()
        {
            _sut = new PassiveProfile(Guid.NewGuid(),
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                DateTimeOffset.UtcNow,
                TrimmedString.Create("user account canceled"));
        }

        [Fact]
        public void have_a_DeactivationDate_property()
        {
            _sut.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Fact]
        public void have_a_Reason_property()
        {
            _sut.Reason.ToString().Should().Be("user account canceled");
        }

        public static IEnumerable<object[]> InvalidArgumentsForPassiveProfile => new List<object[]>
        {
            new object[] { Guid.NewGuid(), Guid.NewGuid(), _email, _contactDetails, _locationDetails, _geographicLocation, _userPreferences, default, TrimmedString.Create("adasdasd") },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), _email, _contactDetails, _locationDetails, _geographicLocation, _userPreferences, DateTimeOffset.Now, null},
        };

        [Theory]
        [MemberData(nameof(InvalidArgumentsForPassiveProfile))]
        public void thrown_an_exception_during_creation_if_arguments_are_not_valid_for_passive_profile(Guid id,
            Guid userId,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences,
            DateTimeOffset deactivationDate,
            TrimmedString reason
            )
        {
            Action createAction = () => new PassiveProfile(id,
                userId,
                email,
                contactDetails,
                locationDetails,
                geographicLocation,
                userPreferences,
                deactivationDate,
                reason);

            createAction.Should().Throw<ArgumentNullException>();
        }
    }
}
