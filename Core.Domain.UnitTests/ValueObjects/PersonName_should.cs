using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class PersonName_should
    {
        [Fact]
        public void have_capitalized_FirstName_property()
        {
            Either<Error, PersonName> eitherPersonName = PersonName.Create("jane", "doe");

            eitherPersonName
                .Right(personName => personName.FirstName.ToString().Should().Be("Jane"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_capitalized_LastName_property()
        {
            Either<Error, PersonName> eitherPersonName = PersonName.Create("will", "smith");

            eitherPersonName
                .Right(personName => personName.LastName.ToString().Should().Be("Smith"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_capitalized_FullName_property()
        {
            Either<Error, PersonName> eitherPersonName = PersonName.Create("ddd", "eee");

            eitherPersonName
               .Right(personName => personName.FullName.ToString().Should().Be("Ddd Eee"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_FirstNames_and_LastNames_match()
        {
            var first = PersonName.Create("mickey", "mouse");
            var second = PersonName.Create("mickey", "mouse");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_FirstNames_and_LastNames_match()
        {
            var first = (object)PersonName.Create("mickey", "mouse");
            var second = (object)PersonName.Create("mickey", "mouse");

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
