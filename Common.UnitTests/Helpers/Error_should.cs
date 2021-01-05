using Common.Helpers;
using FluentAssertions;
using Xunit;

namespace Common.UnitTests.Helpers
{
    public class Error_should
    {
        [Fact]
        public void have_an_internal_NotFound_class()
        {
            var error = new Error.NotFound("not found error message");

            error.Should().NotBeNull();
            error.Message.Should().Be("not found error message");
        }

        [Fact]
        public void have_a_Message_property_on_base_type()
        {
            var error = new Error.NotFound("asd");
            var baseError = error as Error;

            baseError.Should().NotBeNull();
            baseError.Message.Should().Be("asd");
        }

        [Fact]
        public void have_an_internal_Invalid_class()
        {
            var error = new Error.Invalid("somethings not right");

            error.Should().NotBeNull();
            error.Message.Should().Be("somethings not right");
        }

        [Fact]
        public void have_an_internal_Unauthorized_class()
        {
            var error = new Error.Unauthorized("forbidden");

            error.Should().NotBeNull();
            error.Message.Should().Be("forbidden");
        }
    }
}
