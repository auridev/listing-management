using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class LocationDetails_should
    {
        private readonly Either<Error, LocationDetails> _sut = LocationDetails.Create("al", "staaaat", "polis", "aaa1", "some random place 12");

        [Fact]
        public void have_a_CountryCode_property()
        {
            _sut
                .Right(details => details.CountryCode.Value.Should().Be("AL"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_State_property()
        {
            _sut
                .Right(details => details.State.Some(state => state.Name.Value.Should().Be("Staaaat")))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_City_property()
        {
            _sut
                .Right(details => details.City.Name.ToString().Should().Be("Polis"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_PostCode_property()
        {
            _sut
                .Right(details => details.PostCode.Value.ToString().Should().Be("aaa1"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_an_Address_property()
        {
            _sut
                .Right(details => details.Address.Value.ToString().Should().Be("some random place 12"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void not_require_State_property()
        {
            LocationDetails
                .Create("al", string.Empty, "polis", "aaa1", "some random place 12")
                .Right(details => details.State.IsNone.Should().BeTrue())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_all_values_match()
        {
            var first = LocationDetails.Create("pl", "aw", "warshaw", "12", "ddddd");
            var second = LocationDetails.Create("pl", "aw", "warshaw", "12", "ddddd");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_all_values_match()
        {
            var first = (object)LocationDetails.Create("pl", "aw", "warshaw", "12", "ddddd");
            var second = (object)LocationDetails.Create("pl", "aw", "warshaw", "12", "ddddd");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_all_values_match()
        {
            var first = LocationDetails.Create("us", "123", "la", "la123", "under a bridge");
            var second = LocationDetails.Create("us", "123", "la", "la123", "under a bridge");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_any_value_doesnt_match()
        {
            var first = LocationDetails.Create("us", "123", "la", "la123", "under a bridge");
            var second = LocationDetails.Create("us", "1234", "la", "la123", "under a bridge");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
