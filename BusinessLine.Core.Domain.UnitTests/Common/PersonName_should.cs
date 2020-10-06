using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class PersonName_should
    {
        [Fact]
        public void have_a_FirstName_property()
        {
            var personName = PersonName.Create("John", "Smith");
            personName.FirstName.ToString().Should().Be("John");
        }

        [Fact]
        public void have_capitalized_FirstName_property()
        {
            var personName = PersonName.Create("jane", "doe");
            personName.FirstName.ToString().Should().Be("Jane");
        }

        [Fact]
        public void have_a_LastName_property()
        {
            var personName = PersonName.Create("Harry", "Potter");
            personName.LastName.ToString().Should().Be("Potter");
        }

        [Fact]
        public void have_capitalized_LastName_property()
        {
            var personName = PersonName.Create("will", "smith");
            personName.LastName.ToString().Should().Be("Smith");
        }

        [Fact]
        public void have_a_capitalized_FullName_property()
        {
            var personName = PersonName.Create("ddd", "eee");
            personName.FullName.Should().Be("Ddd Eee");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_FirstNames_and_LastNames_match()
        {
            var first = PersonName.Create("mickey", "mouse");
            var second = PersonName.Create("mickey", "mouse");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_FirstNames_and_LastNames_match()
        {
            var first = PersonName.Create("minnie", "mouse");
            var second = PersonName.Create("minnie", "mouse");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Theory]
        [InlineData("mick", "jagger", "john", "lennon")]
        [InlineData("mick", "jagger", "mick", "lennon")]
        [InlineData("john", "jagger", "john", "lennon")]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_either_FirstNames_or_LastNames_dont_match
        (
            string firstFirstName, 
            string firstLastName, 
            string secondFirstName, 
            string secondLastName
        )
        {
            var first = PersonName.Create(firstFirstName, firstLastName);
            var second = PersonName.Create(secondFirstName, secondLastName);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
