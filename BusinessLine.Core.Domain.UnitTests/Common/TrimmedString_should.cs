using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class TrimmedString_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var trimmedString = TrimmedString.Create("valio");
            trimmedString.Value.Should().Be("valio");
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void throw_an_exception_during_creation_if_value_is_not_valid(string value)
        {
            Action createAction = () => TrimmedString.Create(value);

            createAction.Should().Throw<ArgumentException>();
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
            var trimmedString = TrimmedString.Create("  qwerty     qwerty   ");

            trimmedString.Value.Should().Be("qwerty     qwerty");
        }

        [Fact]
        public void have_implicit_cast_to_string()
        {
            string str = TrimmedString.Create("a");

            str.Should().Be("a");
        }

        [Fact]
        public void have_explicit_cast_from_string()
        {
            string str = "123";
            TrimmedString trimmedString = (TrimmedString)str;

            trimmedString.Value.Should().Be("123");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_match()
        {
            var first = (TrimmedString)"my string";
            var second = (TrimmedString)"my string";
            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = (TrimmedString)"my string 2";
            var second = (TrimmedString)"my string 2";
            var equals = first == (second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = (TrimmedString)"my";
            var second = (TrimmedString)"my string";
            var nonEquals = first != (second);

            nonEquals.Should().BeTrue();
        }

        [Fact]
        public void have_a_static_None_property()
        {
            var none = TrimmedString.None;

            none.Should().NotBeNull();
        }

        [Fact]
        public void have_None_with_value_of_empty()
        {
            var none = TrimmedString.None;

            none.Value.Should().Be(string.Empty);
        }
    }
}
