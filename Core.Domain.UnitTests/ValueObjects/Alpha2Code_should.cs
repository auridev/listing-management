using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Alpha2Code_should
    {
        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Value_property()
        {
            Either<Error, Alpha2Code> eitherCode = Alpha2Code.Create("     AB  ");

            eitherCode
               .Right(code => code.Value.Should().Be("AB"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Value_in_capital_letters()
        {
            Either<Error, Alpha2Code> eitherCode = Alpha2Code.Create("as");

            eitherCode
               .Right(code => code.Value.Should().Be("AS"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Arguments))]
        public void return_EiherLeft_with_proper_error_during_creation_if_value_is_not_valid(string code)
        {
            Either<Error, Alpha2Code> eitherCode = Alpha2Code.Create(code);

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
            new object[] { "ASD" },
            new object[] { "A" }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_match()
        {
            var first = Alpha2Code.Create("as");
            var second = Alpha2Code.Create("AS");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_match()
        {
            var first = (object)Alpha2Code.Create("  us");
            var second = (object)Alpha2Code.Create("US   ");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = Alpha2Code.Create("lt");
            var second = Alpha2Code.Create("lt");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Alpha2Code.Create("it");
            var second = Alpha2Code.Create("lt");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
