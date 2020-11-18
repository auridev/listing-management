using Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Country_should
    {
        private readonly Country _sut;
       
        public Country_should()
        {
            _sut = Country.Create("lithuania", 
                Alpha2Code.Create("lt"), 
                Alpha3Code.Create("ltu"),
                Currency.Euro);
        }

        [Fact]
        public void have_a_Alpha2Code_property()
        {
            _sut.Alpha2Code.Value.Should().Be("LT");
        }

        [Fact]
        public void have_a_Alpha3Code_property()
        {
            _sut.Alpha3Code.Value.Should().Be("LTU");
        }

        [Fact]
        public void have_a_Name_property()
        {
            _sut.Name.Should().Be("Lithuania");
        }

        [Fact]
        public void have_a_Currency_property()
        {
            _sut.Currency.Should().Be(Currency.Euro);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void throw_an_exception_during_creation_if_values_are_not_valid(string name, string alpha2, string alpha3, Currency currency)
        {
            Action createAction = () => Country.Create(name, Alpha2Code.Create(alpha2), Alpha3Code.Create(alpha3), currency);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { null, null, null, null, },
            new object[] { string.Empty, string.Empty, string.Empty, Currency.Euro },
            new object[] { "", "", "", Currency.Euro },
            new object[] { default, default, default, Currency.Euro },
            new object[] { default, null,  "asdasd", Currency.Euro },
            new object[] { default, "asdasd",  default, Currency.Euro },
            new object[] { "1232123", null, null, Currency.Euro }
        };

        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Name_property()
        {
            var country = Country.Create("  united states  ", Alpha2Code.Create("us"), Alpha3Code.Create("stt"), Currency.Euro);
            country.Name.Should().Be("United States");
        }

        [Fact]
        public void have_capitalized_first_letter_in_each_Name_word()
        {
            var country = Country.Create("united kingdom", Alpha2Code.Create("as"), Alpha3Code.Create("fgg"), Currency.Euro);

            country.Name.Should().Be("United Kingdom");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Names_and_Codes_match()
        {
            var first = Country.Create("c1country", Alpha2Code.Create("c2"), Alpha3Code.Create("c33"), Currency.Euro);
            var second = Country.Create("c1country", Alpha2Code.Create("c2"), Alpha3Code.Create("c33"), Currency.Euro);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Names_and_Codes_match()
        {
            var first = Country.Create("LT", Alpha2Code.Create("AA"), Alpha3Code.Create("bbb"), Currency.Euro);
            var second = Country.Create("LT", Alpha2Code.Create("AA"), Alpha3Code.Create("bbb"), Currency.Euro);

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
            var first = Country.Create(firstName, Alpha2Code.Create(firstAlpha2Code), Alpha3Code.Create(firstAlpha3Code), Currency.Euro);
            var second = Country.Create(secondName, Alpha2Code.Create(secondAlpha2Code), Alpha3Code.Create(secondAlpha3Code), Currency.Euro);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
