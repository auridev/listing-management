using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class ListingDetails_should
    {
        private readonly Either<Error, ListingDetails> _sut = ListingDetails.Create("my stuff", 60, 2.5F, "kg", "my very very cool stuff");

        [Fact]
        public void have_a_Title_property()
        {
            _sut
                .Right(details => details.Title.Value.ToString().Should().Be("my stuff"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_MaterialType_property()
        {
            _sut
               .Right(details => details.MaterialType.Should().Be(MaterialType.Electronics))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_Weight_property()
        {
            _sut
              .Right(details => details.Weight.Value.Should().Be(2.5F))
              .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_a_Description_property()
        {
            _sut
               .Right(details => details.Description.Value.ToString().Should().Be("my very very cool stuff"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_all_values_match()
        {
            var first = ListingDetails.Create("my stuff", 60, 2.5F, "kg", "my very very cool stuff");
            var second = ListingDetails.Create("my stuff", 60, 2.5F, "kg", "my very very cool stuff");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_all_values_match()
        {
            var first = (object)ListingDetails.Create("my stuff", 60, 2.5F, "kg", "my very very cool stuff");
            var second = (object)ListingDetails.Create("my stuff", 60, 2.5F, "kg", "my very very cool stuff");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_all_values_match()
        {
            var first = ListingDetails.Create("my stuff", 60, 2.5F, "kg", "my very very cool stuff");
            var second = ListingDetails.Create("my stuff", 60, 2.5F, "kg", "my very very cool stuff");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_any_value_doesnt_match()
        {
            var first = ListingDetails.Create("my stuff", 60, 2.5F, "kg", "my very very cool stuff");
            var second = ListingDetails.Create("my stufffffff", 60, 2.5F, "kg", "my very very cool stuff");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
