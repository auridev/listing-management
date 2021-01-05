using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class TrimmedString_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            Either<Error, TrimmedString> eitherTrimmedString = TrimmedString.Create("valio");

            eitherTrimmedString.IsRight.Should().BeTrue();
            eitherTrimmedString
                .Right(trimmedString => trimmedString.Value.Should().Be("valio"))
                .Left(_ => throw new InvalidOperationException());
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void return_Eiher_in_Left_state_with_proper_error_during_creation_if_value_is_not_valid(string value)
        {
            Either<Error, TrimmedString> eitherTrimmedString = TrimmedString.Create(value);

            eitherTrimmedString.IsLeft.Should().BeTrue();
            eitherTrimmedString
                .Right(_ => throw new InvalidOperationException("shouldn't reach this code"))
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "" },
            new object[] { default }
        };

        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Name_property()
        {
            Either<Error, TrimmedString> eitherTrimmedString = TrimmedString.Create("  qwerty     qwerty   ");

            eitherTrimmedString
                .Right(trimmedString => trimmedString.Value.Should().Be("qwerty     qwerty"))
                .Left(_ => throw new InvalidOperationException());
        }

        [Fact]
        public void have_implicit_cast_to_string()
        {
            Either<Error, TrimmedString> eitherTrimmedString = TrimmedString.Create("a");

            eitherTrimmedString
                .Right(trimmedString =>
                {
                    string str = trimmedString;
                    str.Should().Be("a");
                })
                .Left(_ => throw new InvalidOperationException());
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_match()
        {
            var first = TrimmedString.Create("my string");
            var second = TrimmedString.Create("my string");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_match()
        {
            var first = (object)TrimmedString.Create("my string");
            var second = (object)TrimmedString.Create("my string");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = TrimmedString.Create("my string 2");
            var second = TrimmedString.Create("my string 2");
            var equals = first == (second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = TrimmedString.Create("my");
            var second = TrimmedString.Create("my string");
            var nonEquals = first != (second);

            nonEquals.Should().BeTrue();
        }
    }
}
