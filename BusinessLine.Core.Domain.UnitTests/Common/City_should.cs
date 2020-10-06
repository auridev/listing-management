using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class City_should
    {
        [Fact]
        public void have_a_Name_property()
        {
            var city = City.Create("vilnius");
            city.Name.ToString().Should().Be("Vilnius");
        }

        [Fact]
        public void have_capitalized_first_letter_in_each_Name_word()
        {
            var city = City.Create("new york city");

            city.Name.ToString().Should().Be("New York City");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Names_match()
        {
            var first = City.Create("Kaunas");
            var second = City.Create("Kaunas");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Names_match()
        {
            var first = City.Create("  london");
            var second = City.Create("london  ");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Names_dont_match()
        {
            var first = City.Create("berlin");
            var second = City.Create("hamburg");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
