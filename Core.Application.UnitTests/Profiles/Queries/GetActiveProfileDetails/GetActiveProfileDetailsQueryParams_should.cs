using Core.Application.Profiles.Queries.GetActiveProfileDetails;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Queries.GetProfileDetails
{
    public class GetActiveProfileDetailsQueryParams_should
    {
        [Fact]
        public void have_a_ProfileId_property()
        {
            var profileId = Guid.NewGuid();
            var sut = new GetActiveProfileDetailsQueryParams()
            {
                ProfileId = profileId
            };

            sut.ProfileId.Should().Be(profileId);
        }
    }
}
