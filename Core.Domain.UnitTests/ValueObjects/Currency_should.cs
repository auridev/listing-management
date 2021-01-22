using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Currency_should
    {
        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Symbol_property()
        {
            Currency
                .Create("132", " e ", "sss")
                .Right(currency => currency.Symbol.Should().Be("e"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Name_property()
        {
            Currency
                .Create("wer", "T", "   fddd ")
                .Right(currency => currency.Name.Should().Be("fddd"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_Code_property()
        {
            Currency
                .Create("cod", "symbol", "name")
                .Right(currency => currency.Code.Value.Should().Be("COD"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_predefined_Euro_property_matching_Euro_currency()
        {
            Currency
                .Create("EUR", "€", "Euro")
                .Right(currency => currency.Should().Be(Currency.Euro))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_predefined_USDollar_property_matching_US_dollar_currency()
        {
            Currency
                .Create("USD", "$", "US Dollar")
                .Right(currency => currency.Should().Be(Currency.USDollar))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_predefined_PolishZloty_property_matching_Polish_zloty_currency()
        {
            Currency
                .Create("PLN", "zł", "PZloty")
                .Right(currency => currency.Should().Be(Currency.PolishZloty))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_predefined_PoundSterling_property_matching_Pound_Sterling_currency()
        {
            Currency
                .Create("GBP", "£", "Pound Sterling")
                .Right(currency => currency.Should().Be(Currency.PoundSterling))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void return_EiherLeft_with_proper_error_during_creation_if_value_is_not_valid(string code, string symbol, string name)
        {
            Either<Error, Currency> eitherCurrency = Currency.Create(code, symbol, name);

            eitherCurrency.IsLeft.Should().BeTrue();
            eitherCurrency
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { null, null, null },
            new object[] { null, string.Empty, string.Empty  },
            new object[] { null, "", "" },
            new object[] { default, default, default },
            new object[] { default, "asdasd", null },
            new object[] { "asd", null, null },
            new object[] { null, null, "khjkjhj" }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Codes_and_Symbols_match()
        {
            var first = Currency.Create("eur", "c", "euro");
            var second = Currency.Create("eur", "c", "euro");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Names_match()
        {
            var first = (object)Currency.Create("eur", "c", "euro");
            var second = (object)Currency.Create("eur", "c", "euro");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Codes_and_Symbols_match()
        {
            var first = Currency.Create("usd", "s", "dollar");
            var second = Currency.Create("usd", "s", "dollar");

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
            var first = Currency.Create(firstCode, firstSymbol, firstName);
            var second = Currency.Create(secondCode, secondSymbol, secondName);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
