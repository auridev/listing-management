using Core.Domain.Common;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Subject_should
    {
        private readonly Subject _sut;

        public Subject_should()
        {
            _sut = Subject.Create("my dear friend");
        }

        [Fact]
        public void have_Value_property()
        {
            _sut.Value.ToString().Should().NotBeNull();
        }

        [Fact]
        public void have_capitalized_first_Value_letter()
        {
            _sut.Value.ToString().Should().Be("My dear friend");
        }

        [Fact]
        public void throw_exception_if_argument_is_not_valid()
        {
            Action action = () => Subject.Create(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = Subject.Create("a b c");
            var second = Subject.Create("a b c");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object) Subject.Create("a b c");
            var second = (object) Subject.Create("a b c");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equality_operator_if_values_match()
        {
            var first = Subject.Create("a b c");
            var second = Subject.Create("a b c");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_not_equals_operator_if_values_dont_match()
        {
            var first = Subject.Create("a b c");
            var second = Subject.Create("a b c b");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
