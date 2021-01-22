using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class MonetaryValue_should
    {
        private readonly string _currencyCode = "AAA";

        [Fact]
        public void have_Value_property()
        {
            MonetaryValue
                .Create(20M, _currencyCode)
                .Right(value => value.Value.Should().Be(20M))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_CurrencyCode_property()
        {
            MonetaryValue
                .Create(1000M, _currencyCode)
                .Right(value => value.CurrencyCode.Value.Should().Be(_currencyCode))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid(decimal value)
        {
            Either<Error, MonetaryValue> eitherMonetaryValue = MonetaryValue.Create(value, _currencyCode);

            eitherMonetaryValue.IsLeft.Should().BeTrue();
            eitherMonetaryValue
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { 0 },
            new object[] { -1 },
            new object[] { -1000M },
            new object[] { -1.2M }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_and_Currencies_match()
        {
            var first = MonetaryValue.Create(2.3M, "USD");
            var second = MonetaryValue.Create(2.3M, "USD");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_and_Currencies_match()
        {
            var first = (object)MonetaryValue.Create(2.3M, "USD");
            var second = (object)MonetaryValue.Create(2.3M, "USD");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_and_Currencies_match()
        {
            var first = MonetaryValue.Create(8M, "EUR");
            var second = MonetaryValue.Create(8M, "EUR");

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
            var first = MonetaryValue.Create(firstValue, firstCurrencyCode);
            var second = MonetaryValue.Create(secondValue, secondCurrencyCode);

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
