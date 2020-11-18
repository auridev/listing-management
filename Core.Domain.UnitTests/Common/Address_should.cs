using Core.Domain.Common;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Address_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var address = Address.Create("gariunai");
            address.Value.ToString().Should().Be("gariunai");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_match()
        {
            var first = Address.Create("my place");
            var second = Address.Create("my place");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = Address.Create("aaa");
            var second = Address.Create("aaa");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Address.Create("aaa");
            var second = Address.Create("bbb");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
