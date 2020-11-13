using Core.Application.Profiles.Queries.GetProfileList;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Queries.GetProfileList
{
    public class GetProfileListQueryParams_should
    {
        private readonly GetProfileListQueryParams _sut;

        public GetProfileListQueryParams_should()
        {
            _sut = new GetProfileListQueryParams()
            {
                Search = "aaa",
                IsActive = true
            };
        }

        [Fact]
        public void have_a_Search_property()
        {
            _sut.Search.Should().NotBeNull();
        }

        [Fact]
        public void have_a_IsActive_property()
        {
            _sut.IsActive.HasValue.Should().BeTrue();
            _sut.IsActive.Should().BeTrue();
        }
    }
}
