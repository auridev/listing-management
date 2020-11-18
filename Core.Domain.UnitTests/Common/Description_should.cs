using Core.Domain.Common;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Description_should
    {
        [Fact]
        public void have_a_non_default_string_Value_property()
        {
            var description = Description.Create("my description");

            description.Value.ToString().Should().Be("my description");
        }


        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_match()
        {
            var first = Description.Create("cat for sale");
            var second = Description.Create("cat for sale");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = Description.Create("cat not for sale");
            var second = Description.Create("cat not for sale");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Description.Create("cat for sale");
            var second = Description.Create("dog not for sale");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
