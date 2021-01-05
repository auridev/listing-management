using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class State_should
    {
        [Fact]
        public void have_a_Name_property()
        {
            var state = State.Create("qqq");
            state.Name.ToString().Should().Be("Qqq");
        }

        [Fact]
        public void have_capitalized_first_letter_in_each_Name_word()
        {
            var state = State.Create("vvv bbb nnn mmm");

            state.Name.ToString().Should().Be("Vvv Bbb Nnn Mmm");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Names_match()
        {
            var first = State.Create("zzzzz");
            var second = State.Create("zzzzz");

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
