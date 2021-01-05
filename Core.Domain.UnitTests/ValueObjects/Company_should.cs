using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Company_should
    {
        [Fact]
        public void have_a_Name_property()
        {
            Either<Error, Company> eitherCompany = Company.Create("some name");

            eitherCompany
               .Right(company => company.Name.ToString().Should().Be("some name"))
               .Left(_ => throw new InvalidOperationException());
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Names_match()
        {
            var first = Company.Create("asd");
            var second = Company.Create("asd");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Names_match()
        {
            var first = (object)Company.Create("asd");
            var second = (object)Company.Create("asd");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Names_match()
        {
            var first = Company.Create("qwe");
            var second = Company.Create("qwe");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Names_dont_match()
        {
            var first = Company.Create("rt");
            var second = Company.Create("yu");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
