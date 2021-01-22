using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Subject_should
    {
        [Fact]
        public void have_capitalized_first_Value_letter()
        {
            Subject
                .Create("my dear friend")
                .Right(subject => subject.Value.ToString().Should().Be("My dear friend"))
                .Left(_ => throw InvalidExecutionPath.Exception);
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
