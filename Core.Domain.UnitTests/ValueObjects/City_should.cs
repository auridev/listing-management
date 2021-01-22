using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class City_should
    {
        [Fact]
        public void have_capitalized_first_letter_in_each_Name_word()
        {
            Either<Error, City> eitherCity = City.Create("new york city");

            eitherCity
                .Right(city => city.Name.ToString().Should().Be("New York City"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Names_match()
        {
            var first = City.Create("Kaunas");
            var second = City.Create("Kaunas");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Names_match()
        {
            var first = (object)City.Create("Kaunas");
            var second = (object)City.Create("Kaunas");

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
