using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class CurrencyCode_should
    {
        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Value_property()
        {
            CurrencyCode
                .Create("     BMD  ")
                .Right(code => code.Value.Should().Be("BMD"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Value_in_capital_letters()
        {
            CurrencyCode
                .Create("bov")
                .Right(code => code.Value.Should().Be("BOV"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Arguments))]
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid(string code)
        {
            Either<Error, CurrencyCode> eitherCurrencyCode = CurrencyCode.Create(code);

            eitherCurrencyCode.IsLeft.Should().BeTrue();
            eitherCurrencyCode
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
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
            var first = (object)CurrencyCode.Create("  EUR");
            var second = (object)CurrencyCode.Create("EUR   ");

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
