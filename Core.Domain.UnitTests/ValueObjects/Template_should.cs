using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Template_should
    {
        [Fact]
        public void have_Value_property()
        {
            Template
                .Create("hello form the other side")
                .Right(template => template.Value.Should().Be("hello form the other side"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = Template.Create("hello form the other side");
            var second = Template.Create("hello form the other side");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object)Template.Create("hello form the other side");
            var second = (object)Template.Create("hello form the other side");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_values_match()
        {
            var first = Template.Create("hello form the other side");
            var second = Template.Create("hello form the other side");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_values_dont_match()
        {
            var first = Template.Create("hello form the other side");
            var second = Template.Create("hello form the other sideeeeee");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid(string value)
        {
            Either<Error, Template> eitherTemplate = Template.Create(value);

            eitherTemplate.IsLeft.Should().BeTrue();
            eitherTemplate
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "" },
            new object[] { default }
        };
    }
}
