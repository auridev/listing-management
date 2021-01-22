using Common.Helpers;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;
using static LanguageExt.Prelude;

namespace Common.UnitTests.Helpers
{
    public class StringHelpers_should
    {
        [Fact]
        public void have_EnsureNonEmpty_method()
        {
            var result = Functions.EnsureNonEmpty("asd");

            result.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(InvalidStrings))]
        public void return_Eiher_in_Left_state_if_invalid_argument_has_been_passed_to_EnsureNonEmpty(string argument)
        {
            Either<Error, string> result = Functions.EnsureNonEmpty(argument);

            result.IsLeft.Should().BeTrue();
            result
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> InvalidStrings => new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "" },
            new object[] { default }
        };

        [Theory]
        [InlineData("1")]
        [InlineData("asd")]
        [InlineData("..")]
        public void return_Eiher_in_Right_state_if_valid_argument_has_been_passed_to_EnsureNonEmpty(string argument)
        {
            Either<Error, string> result = Functions.EnsureNonEmpty(argument);

            result.IsRight.Should().BeTrue();
            result
                .Right(value => value.Should().Be(argument))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Trim_method()
        {
            var result = Functions.Trim(Right("asd"));

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_Either_in_Right_state_if_valid_argument_has_been_passed_to_Trim()
        {
            Either<Error, string> result = Functions.Trim(Right("asd"));

            result.IsRight.Should().BeTrue();
        }

        [Fact]
        public void return_string_without_leading_and_trailing_spaces_from_Trim()
        {
            Either<Error, string> result = Functions.Trim(Right("  nmk    "));

            result
                .Right(value => value.Should().Be("nmk"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void return_Either_in_Left_state_if_invalid_argument_has_been_passed_to_Trim()
        {
            Either<Error, string> input = Left<Error, string>(new Error.Invalid("invalid string"));

            Either<Error, string> result = Functions.Trim(input);

            result.IsLeft.Should().BeTrue();
        }

        [Fact]
        public void have_ConvertToUpper_method()
        {
            var result = Functions.ConvertToUpper(Right("qwerty"));

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_Either_in_Right_state_if_valid_argument_has_been_passed_to_ConvertToUpper()
        {
            Either<Error, string> result = Functions.ConvertToUpper(Right("qwerty"));

            result.IsRight.Should().BeTrue();
        }

        [Fact]
        public void return_string_in_uppercase_from_ConvertToUpper()
        {
            Either<Error, string> result = Functions.ConvertToUpper(Right("qwerty"));

            result
                .Right(value => value.Should().Be("QWERTY"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void return_Either_in_Left_state_if_invalid_argument_has_been_passed_to_ConvertToUpper()
        {
            Either<Error, string> input = Left<Error, string>(new Error.Invalid("invalid string"));

            Either<Error, string> result = Functions.ConvertToUpper(input);

            result.IsLeft.Should().BeTrue();
        }

        [Fact]
        public void have_EnsureRequiredLength_method()
        {
            var result = Functions.EnsureRequiredLength("asd", 3);

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_Eiher_in_Left_state_if_invalid_argument_has_been_passed_to_EnsureRequiredLength()
        {
            Either<Error, string> result = Functions.EnsureRequiredLength(null, 1);

            result.IsLeft.Should().BeTrue();
            result
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Theory]
        [InlineData("a", 1, true)]
        [InlineData("aa", 2, true)]
        [InlineData("aaa", 10, false)]
        public void return_Eiher_in_Right_state_if_valid_argument_has_been_passed_to_EnsureRequiredLength(string input, int length, bool shouldBeRight)
        {
            Either<Error, string> result = Functions.EnsureRequiredLength(input, length);

            result.IsRight.Should().Be(shouldBeRight);
        }

        [Fact]
        public void have_CapitalizeAllWords_method()
        {
            var result = Functions.CapitalizeAllWords("a b c");

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_EiherRight_with_first_letter_of_each_word_capitalised_from_CapitalizeAllWords()
        {
            Either<Error, string> result = Functions.CapitalizeAllWords("one two");

            result.IsRight.Should().BeTrue();
            result
                .Right(value => value.Should().Be("One Two"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_CapitalizeFirstLetter_method()
        {
            var result = Functions.CapitalizeFirstLetter("qwe");

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_EiherRight_with_only_first_letter_capitalised_from_CapitalizeFirstLetter()
        {
            Either<Error, string> result = Functions.CapitalizeFirstLetter("first second");

            result.IsRight.Should().BeTrue();
            result
                .Right(value => value.Should().Be("First second"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_EnsureMinLength_method()
        {
            var result = Functions.EnsureMinLength("asd", 2);

            result.Should().NotBeNull();
        }

        [Fact]
        public void return_EiherLeft_if_invalid_argument_has_been_passed_to_EnsureMinLength()
        {
            Either<Error, string> result = Functions.EnsureMinLength(null, 1);

            result.IsLeft.Should().BeTrue();
            result
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Theory]
        [InlineData("b", 1, true)]
        [InlineData("bb", 1, true)]
        [InlineData("bb", 3, false)]
        public void return_EiherRight_if_valid_argument_has_been_passed_to_EnsureMinLength(string input, int length, bool shouldBeRight)
        {
            Either<Error, string> result = Functions.EnsureMinLength(input, length);

            result.IsRight.Should().Be(shouldBeRight);
        }
    }
}
