using Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Email_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var email = Email.Create("a@aa.com");
            email.Value.Should().Be("a@aa.com");
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void throw_an_exception_during_creation_if_value_is_not_valid(string value)
        {
            Action createAction = () => Email.Create(value);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "" },
            new object[] { default },
            new object[] { "aaaaa" },
            new object[] { "f@" },
            new object[] { "@" },
            new object[] { "@gg.com" },
            new object[] { "@." },
            new object[] { "a@." },
            new object[] { "a@com" }
        };

        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Name_property()
        {
            var email = Email.Create("  qqq@aaa.lt   ");

            email.Value.Should().Be("qqq@aaa.lt");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_match()
        {
            var first = Email.Create("aaa@google.com");
            var second = Email.Create("aaa@google.com");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = Email.Create("we@microsoft.org");
            var second = Email.Create("we@microsoft.org");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Email.Create("some@eta.net");
            var second = Email.Create("none@eta.net");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
