using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Country_should
    {
        private readonly Either<Error, Country> _sut;

        public Country_should()
        {
            _sut = Country.Create("  united states  ", "us", "stt", Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);
        }

        [Fact]
        public void have_a_Alpha2Code_property()
        {
            _sut
               .Right(c => c.Alpha2Code.Value.Should().Be("US"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_Alpha3Code_property()
        {
            _sut
               .Right(c => c.Alpha3Code.Value.Should().Be("STT"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_Currency_property()
        {
            _sut
               .Right(c => c.Currency.Should().Be(Currency.Euro))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }
        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Name_property()
        {
            _sut
               .Right(c => c.Name.Should().Be("United States"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_capitalized_first_letter_in_each_Name_word()
        {
            _sut
               .Right(c => c.Name.Should().Be("United States"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void return_EiherLeft_with_proper_error_during_creation_if_arguments_are_not_valid(
            string name,
            string alpha2Code,
            string alpha3Code,
            string currencyCode,
            string currencySymbol,
            string currencyName)
        {
            Either<Error, Country> eitherCountry = Country.Create(name, alpha2Code, alpha3Code, currencyCode, currencySymbol, currencyName);

            eitherCountry.IsLeft.Should().BeTrue();
            eitherCountry
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { null, null, null, null, null, null},
            new object[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty },
            new object[] { "", "", "", "", "", "" },
            new object[] { default, default, default, default, default, default },
            new object[] { default, null,  "asdasd", null, null, null },
            new object[] { default, "asdasd",  default, null, null, null },
            new object[] { "1232123", null, null, null, null, null },
            new object[] { null, null,  null, "USD", null, null },
            new object[] { null, null, null, null, "A", null},
            new object[] { null, null, null, null, null, "aaaaa"}
        };

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Names_and_Codes_match()
        {
            var first = Country.Create("c1country", "c2", "c33", Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);
            var second = Country.Create("c1country", "c2", "c33", Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Names_and_Codes_match()
        {
            var first = (object)Country.Create("c1country", "c2", "c33", Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);
            var second = (object)Country.Create("c1country", "c2", "c33", Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Names_and_Codes_match()
        {
            var first = Country.Create("LT", "AA", "bbb", Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);
            var second = Country.Create("LT", "AA", "bbb", Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Theory]
        [InlineData("a_country", "a2", "a33", "b_country", "b2", "b33")]
        [InlineData("a_country", "a2", "a33", "b_country", "a2", "b33")]
        [InlineData("a_country", "a2", "a33", "b_country", "b2", "a33")]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_either_Names_or_Codes_dont_match(string firstName,
            string firstAlpha2Code,
            string firstAlpha3Code,
            string secondName,
            string secondAlpha2Code,
            string secondAlpha3Code)
        {
            var first = Country.Create(firstName, firstAlpha2Code, firstAlpha3Code, Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);
            var second = Country.Create(secondName, secondAlpha2Code, secondAlpha3Code, Currency.Euro.Code.Value, Currency.Euro.Symbol, Currency.Euro.Name);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
