using Core.Domain.ValueObjects;
using FluentAssertions;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Description_should
    {
        [Fact]
        public void have_a_non_default_string_Value_property()
        {
            Description
                .Create("my description")
                .Right(description => description.Value.ToString().Should().Be("my description"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_match()
        {
            var first = Description.Create("cat for sale");
            var second = Description.Create("cat for sale");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_match()
        {
            var first = (object)Description.Create("cat for sale");
            var second = (object)Description.Create("cat for sale");

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
