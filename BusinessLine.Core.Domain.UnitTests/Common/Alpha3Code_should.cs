using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Alpha3Code_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var code = Alpha3Code.Create("ASD");

            code.Value.Should().Be("ASD");
        }

        [Theory]
        [MemberData(nameof(Arguments))]
        public void throw_an_exception_during_creation_if_argument_is_not_valid(string code)
        {
            Action createAction = () => Alpha3Code.Create(code);

            createAction.Should().Throw<ArgumentException>();
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
        public void not_have_any_leading_or_trailing_whitespaces_in_Value_property()
        {
            var code = Alpha3Code.Create("     ABC  ");
            code.Value.Should().Be("ABC");
        }

        [Fact]
        public void have_Value_in_capital_letters()
        {
            var code = Alpha3Code.Create("asd");
            code.Value.Should().Be("ASD");
        }

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
            var first = (object) Alpha3Code.Create("  uss");
            var second = (object) Alpha3Code.Create("USS   ");

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
