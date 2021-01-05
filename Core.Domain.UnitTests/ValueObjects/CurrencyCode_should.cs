using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class CurrencyCode_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var code = CurrencyCode.Create("AED");

            code.Value.Should().Be("AED");
        }

        [Theory]
        [MemberData(nameof(Arguments))]
        public void throw_an_exception_during_creation_if_argument_is_not_valid(string code)
        {
            Action createAction = () => CurrencyCode.Create(code);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> Arguments => new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "" },
            new object[] { default },
            new object[] { "ASDE" },
            new object[] { "AS" },
            new object[] { "A" }
        };

        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Value_property()
        {
            var code = CurrencyCode.Create("     BMD  ");
            code.Value.Should().Be("BMD");
        }

        [Fact]
        public void have_Value_in_capital_letters()
        {
            var code = CurrencyCode.Create("bov");
            code.Value.Should().Be("BOV");
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_match()
        {
            var first = CurrencyCode.Create("DKK");
            var second = CurrencyCode.Create("DKK");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_match()
        {
            var first = (object) CurrencyCode.Create("  EUR");
            var second = (object) CurrencyCode.Create("EUR   ");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = CurrencyCode.Create("GBP");
            var second = CurrencyCode.Create("GBP");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = CurrencyCode.Create("INR");
            var second = CurrencyCode.Create("HKD");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
