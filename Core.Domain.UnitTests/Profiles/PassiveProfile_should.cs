using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Test.Helpers;
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
                _createdDate,
                DateTimeOffset.UtcNow,
                TestValueObjectFactory.CreateTrimmedString("user account canceled"));
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
            new object[] { default, TestValueObjectFactory.CreateTrimmedString("adasdasd") },
            new object[] { DateTimeOffset.Now, null},
        };

        [Theory]
        [MemberData(nameof(InvalidArgumentsForPassiveProfile))]
        public void thrown_an_exception_during_creation_if_arguments_are_not_valid_for_passive_profile(DateTimeOffset deactivationDate, TrimmedString reason)
        {
            Action createAction = () => new PassiveProfile(
                Guid.NewGuid(),
                Guid.NewGuid(),
                _email,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _userPreferences,
                _createdDate,
                deactivationDate,
                reason);

            createAction.Should().Throw<ArgumentNullException>();
        }
    }
}
