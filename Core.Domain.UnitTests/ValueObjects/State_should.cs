using Core.Domain.ValueObjects;
using FluentAssertions;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class State_should
    {
        [Fact]
        public void have_capitalized_first_letter_in_each_Name_word()
        {
            State
                .Create("vvv bbb nnn mmm")
                .Right(state => state.Name.ToString().Should().Be("Vvv Bbb Nnn Mmm"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Names_match()
        {
            var first = State.Create("zzzzz");
            var second = State.Create("zzzzz");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Names_match()
        {
            var first = (object)State.Create("zzzzz");
            var second = (object)State.Create("zzzzz");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Names_match()
        {
            var first = State.Create("ert");
            var second = State.Create("ert");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Names_dont_match()
        {
            var first = State.Create("1");
            var second = State.Create("2");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
