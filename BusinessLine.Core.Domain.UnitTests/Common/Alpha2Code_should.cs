using Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Alpha2Code_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var code = Alpha2Code.Create("BE");

            code.Value.Should().Be("BE");
        }

        [Theory]
        [MemberData(nameof(Arguments))]
        public void throw_an_exception_during_creation_if_argument_is_not_valid(string code)
        {
            Action createAction = () => Alpha2Code.Create(code);

            createAction.Should().Throw<ArgumentException>();
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
        public void not_have_any_leading_or_trailing_whitespaces_in_Value_property()
        {
            var code = Alpha2Code.Create("     AB  ");
            code.Value.Should().Be("AB");
        }

        [Fact]
        public void have_Value_in_capital_letters()
        {
            var code = Alpha2Code.Create("as");
            code.Value.Should().Be("AS");
        }

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
            var first = (object) Alpha2Code.Create("  us");
            var second = (object) Alpha2Code.Create("US   ");

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
