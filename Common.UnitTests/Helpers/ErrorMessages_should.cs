using Common.Helpers;
using FluentAssertions;
using Xunit;

namespace Common.UnitTests.Helpers
{
    public class ErrorMessages_should
    {
        [Fact]
        public void have_CannotBeEmpty_method()
        {
            string message = ErrorMessages.CannotBeEmpty("aaa");

            message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void return_proper_error_messages_from_CannotBeEmpty_method()
        {
            string message = ErrorMessages.CannotBeEmpty("myvar");

            message.Should().Be("myvar cannot be empty");
        }
    }
}
