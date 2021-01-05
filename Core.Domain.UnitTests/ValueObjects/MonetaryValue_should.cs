using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class MonetaryValue_should
    {
        private readonly CurrencyCode _currencyCode;

        public MonetaryValue_should()
        {
            _currencyCode = CurrencyCode.Create("AAA");
        }

        [Fact]
        public void have_a_Value_property()
        {
            var monetaryValue = MonetaryValue.Create(20M, _currencyCode);
            monetaryValue.Value.Should().Be(20M);
        }

        [Fact]
        public void have_a_CurrencyCode_property()
        {
            var monetaryValue = MonetaryValue.Create(1000M, _currencyCode);
            monetaryValue.CurrencyCode.Should().Be(_currencyCode);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void throw_an_exception_during_creation_if_values_are_not_valid(decimal value)
        {
            Action createAction = () => MonetaryValue.Create(value, _currencyCode);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { 0 },
            new object[] { -1 },
            new object[] { -1000M },
            new object[] { -1.2M }
        };
        
        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_and_Currencies_match()
        {
            var first = MonetaryValue.Create(2.3M, 
                CurrencyCode.Create("USD"));
            var second = MonetaryValue.Create(2.3M, 
                CurrencyCode.Create("USD"));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_and_Currencies_match()
        {
            var first = MonetaryValue.Create(8M, 
                CurrencyCode.Create("EUR"));
            var second = MonetaryValue.Create(8M, 
                CurrencyCode.Create("EUR"));

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(ValuesAndCurrencies))]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_either_Values_or_Currencies_dont_match(
            decimal firstValue,
            string firstCurrencyCode,
            decimal secondValue,
            string secondCurrencyCode)
        {
            var first = MonetaryValue.Create(firstValue, 
                CurrencyCode.Create(firstCurrencyCode));
            var second = MonetaryValue.Create(secondValue, 
                CurrencyCode.Create(secondCurrencyCode));

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        public static IEnumerable<object[]> ValuesAndCurrencies => new List<object[]>
        {
            new object[] { 101M, "EUR", 202M, "USD" },
            new object[] { 101M, "EUR", 101M, "USD" },
            new object[] { 101M, "USD", 202M, "USD" }
        };
    }
}
