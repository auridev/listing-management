using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Currency_should
    {
        [Fact]
        public void have_a_Code_property()
        {
            var currency = Currency.Create(CurrencyCode.Create("cod"), "symbol", "name");
            currency.Code.Value.Should().Be("COD");
        }

        [Fact]
        public void have_a_Symbol_property()
        {
            var currency = Currency.Create(CurrencyCode.Create("aaa"), "S", "asdasdads");
            currency.Symbol.Should().Be("S");
        }

        [Fact]
        public void have_a_Name_property()
        {
            var currency = Currency.Create(CurrencyCode.Create("aaa"), "S", "pinigai");
            currency.Name.Should().Be("Pinigai");
        }

        [Fact]
        public void have_a_predefined_Euro_property()
        {
            Currency.Euro.Should().Be(Currency.Create(CurrencyCode.Create("EUR"), "€", "Euro"));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void throw_an_exception_during_creation_if_values_are_not_valid(CurrencyCode code, string symbol, string name)
        {
            Action createAction = () => Currency.Create(code, symbol, name);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { null, null, null },
            new object[] { null, string.Empty, string.Empty  },
            new object[] { null, "", "" },
            new object[] { default, default, default },
            new object[] { default, "asdasd", null },
            new object[] { CurrencyCode.Create("asd"), null, null },
            new object[] { null, null, "khjkjhj" }
        };

        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Symbol_property()
        {
            var currency = Currency.Create(CurrencyCode.Create("132"), " e ", "sss");
            currency.Symbol.Should().Be("e");
        }

        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Name_property()
        {
            var currency = Currency.Create(CurrencyCode.Create("wer"), "T", "   fddd ");
            currency.Name.Should().Be("Fddd");
        }

        [Fact]
        public void have_Name_with_first_capital_letter()
        {
            var currency = Currency.Create(CurrencyCode.Create("ccc"), "s", "name");
            currency.Name.Should().Be("Name");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Codes_and_Symbols_match()
        {
            var first = Currency.Create(CurrencyCode.Create("eur"), "c", "euro");
            var second = Currency.Create(CurrencyCode.Create("eur"), "c", "euro");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Codes_and_Symbols_match()
        {
            var first = Currency.Create(CurrencyCode.Create("usd"), "s", "dollar");
            var second = Currency.Create(CurrencyCode.Create("usd"), "s", "dollar");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Theory]
        [InlineData("USD", "S", "Dollar", "EUR", "C", "Euro")]
        [InlineData("USD", "S", "Dollar", "USD", "C", "Euro")]
        [InlineData("USD", "C", "Dollar", "EUR", "C", "Dollar")]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_either_Codes_or_Symbols_dont_match(string firstCode, 
            string firstSymbol, 
            string firstName, 
            string secondCode, 
            string secondSymbol, 
            string secondName)
        {
            var first = Currency.Create(CurrencyCode.Create(firstCode), firstSymbol, firstName);
            var second = Currency.Create(CurrencyCode.Create(secondCode), secondSymbol, secondName);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
