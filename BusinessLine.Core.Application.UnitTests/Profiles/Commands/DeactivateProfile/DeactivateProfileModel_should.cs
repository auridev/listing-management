using BusinessLine.Core.Application.Profiles.Commands.DeactivateProfile;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.DeactivateProfile
{
    public class DeactivateProfileModel_should
    {
        private readonly DeactivateProfileModel _sut;

        public DeactivateProfileModel_should()
        {
            _sut = new DeactivateProfileModel()
            {
                ActiveProfileId = "adasdadadasd",
                Reason = "because reasons"
            };
        }

        [Fact]
        public void have_an_ActiveProfileId_property()
        {
            _sut.ActiveProfileId.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_Reason_property()
        {
            _sut.Reason.Should().NotBeEmpty();
        }
    }
}
