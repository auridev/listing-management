using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Alpha3Code_should
    {
        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Value_property()
        {
            Either<Error, Alpha3Code> eitherCode = Alpha3Code.Create("     ABC  ");

            eitherCode
              .Right(address => address.Value.Should().Be("ABC"))
              .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Value_in_capital_letters()
        {
            Either<Error, Alpha3Code> eitherCode = Alpha3Code.Create("asd");

            eitherCode
              .Right(address => address.Value.Should().Be("ASD"))
              .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Arguments))]
        public void return_EiherLeft_during_creation_if_argument_is_not_valid(string code)
        {
            Either<Error, Alpha3Code> eitherCode = Alpha3Code.Create(code);

            eitherCode.IsLeft.Should().BeTrue();
            eitherCode
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
            var first = Alpha3Code.Create("ass");
            var second = Alpha3Code.Create("ASS");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_match()
        {
            var first = (object)Alpha3Code.Create("  uss");
            var second = (object)Alpha3Code.Create("USS   ");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = Alpha3Code.Create("ltt");
            var second = Alpha3Code.Create("ltt");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Alpha3Code.Create("ita");
            var second = Alpha3Code.Create("ltz");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
