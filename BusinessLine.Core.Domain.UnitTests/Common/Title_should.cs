using Core.Domain.Common;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Title_should
    {
        [Fact]
        public void have_non_default_string_Value_property()
        {
            var title = Title.Create("my title");

            title.Value.ToString().Should().Be("my title");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_match()
        {
            var first = Title.Create("metal for sale");
            var second = Title.Create("metal for sale");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = Title.Create("wanna sell stuff...");
            var second = Title.Create("wanna sell stuff...");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Title.Create("nein nein nein");
            var second = Title.Create("ja ja ja");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
