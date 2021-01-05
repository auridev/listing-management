using Common.Helpers;
using FluentAssertions;
using LanguageExt;
using Xunit;
using static LanguageExt.Prelude;

namespace Common.UnitTests.Helpers
{
    public class Result_should
    {
        [Fact]
        public void have_static_Success_method()
        {
            var result = Result.Success<Unit>(unit);

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_Either_in_Right_state_when_Success_is_called()
        {
            Either<Error, Unit> result = Result.Success(unit);

            result.IsRight.Should().BeTrue();
            result.IsLeft.Should().BeFalse();
        }

        [Fact]
        public void have_static_Invalid_method()
        {
            var result = Result.Invalid<Error>("aaa");

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_Either_in_Left_state_when_Invalid_is_called()
        {
            Either<Error, Unit> result = Result.Invalid<Unit>("aaa");

            result.IsLeft.Should().BeTrue();
            result.IsRight.Should().BeFalse();
        }

        [Fact]
        public void have_static_NotFound_method()
        {
            var result = Result.NotFound<Unit>("bbb");

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_Either_in_Left_state_when_NotFound_is_called()
        {
            Either<Error, Unit> result = Result.NotFound<Unit>("bbb");

            result.IsLeft.Should().BeTrue();
            result.IsRight.Should().BeFalse();
        }

        [Fact]
        public void have_static_Unauthorized_method()
        {
            var result = Result.Unauthorized<Unit>("ccc");

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_Either_in_Left_state_when_Unauthorized_is_called()
        {
            Either<Error, Unit> result = Result.Unauthorized<Unit>("ccc");

            result.IsLeft.Should().BeTrue();
            result.IsRight.Should().BeFalse();
        }
    }
}
