using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Address_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            Either<Error, Address> eitherAddress = Address.Create("gariunai");

            eitherAddress.IsRight.Should().BeTrue();
            eitherAddress
                .Right(address => address.Value.ToString().Should().Be("gariunai"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_match()
        {
            var first = Address.Create("my place");
            var second = Address.Create("my place");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_match()
        {
            var first = (object)Address.Create("my place");
            var second = (object)Address.Create("my place");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = Address.Create("aaa");
            var second = Address.Create("aaa");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Address.Create("aaa");
            var second = Address.Create("bbb");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
