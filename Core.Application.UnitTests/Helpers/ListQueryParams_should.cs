using Core.Application.Helpers;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Helpers
{
    public class ListQueryParams_should
    {
        private readonly ListQueryParams _sut;

        public ListQueryParams_should()
        {
            _sut = new ListQueryParams();
        }

        [Fact]
        public void have_a_default_PageNumber_property()
        {
            _sut.PageNumber.Should().Be(1);
        }

        [Fact]
        public void have_a_default_PageSize_property()
        {
            _sut.PageSize.Should().Be(10);
        }

        [Fact]
        public void set_page_size_to_max_allowed_when_trying_to_set_above_max()
        {
            // act => set above max
            _sut.PageSize = 50;

            // assert => equals to max allowed
            _sut.PageSize.Should().Be(25);
        }

        [Fact]
        public void have_a_DefaultOffset_property()
        {
            var sut = new ListQueryParams()
            {
                PageNumber = 3,
                PageSize = 20
            };

            sut.DefaultOffset.Should().Be(40);
        }
    }
}
