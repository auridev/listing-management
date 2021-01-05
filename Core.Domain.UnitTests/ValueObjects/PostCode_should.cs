using Core.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class PostCode_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var postCode = PostCode.Create("A2323");
            postCode.Value.ToString().Should().Be("A2323");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_match()
        {
            var first = PostCode.Create("111-222");
            var second = PostCode.Create("111-222");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = PostCode.Create("a2a");
            var second = PostCode.Create("a2a");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = PostCode.Create("eeeee");
            var second = PostCode.Create("aaaaaaaaaa");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
