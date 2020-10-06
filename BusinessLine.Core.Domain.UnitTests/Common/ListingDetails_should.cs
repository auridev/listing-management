using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class ListingDetails_should
    {
        private readonly ListingDetails _sut;

        public ListingDetails_should()
        {
            _sut = ListingDetails.Create(
                Title.Create("my stuff"),
                MaterialType.Electronics,
                Weight.Create(2.5F, MassMeasurementUnit.Kilogram),
                Description.Create("my very very cool stuff")
            );
        }

        [Fact]
        public void have_a_Title_property()
        {
            _sut.Title.Value.ToString().Should().Be("my stuff");

        }

        [Fact]
        public void have_a_MaterialType_property()
        {
            _sut.MaterialType.Should().Be(MaterialType.Electronics);
        }

        [Fact]
        public void have_a_Weight_property()
        {
            _sut.Weight.Should().Be(Weight.Create(2.5F, MassMeasurementUnit.Kilogram));

        }

        [Fact]
        public void have_a_Description_property()
        {
            _sut.Description.Value.ToString().Should().Be("my very very cool stuff");
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_all_values_match()
        {
            var first = ListingDetails.Create
            (
                Title.Create("my stuff"),
                MaterialType.Electronics,
                Weight.Create(2.5F, MassMeasurementUnit.Kilogram),
                Description.Create("my very very cool stuff")
            );
            var second = ListingDetails.Create
            (
                Title.Create("my stuff"),
                MaterialType.Electronics,
                Weight.Create(2.5F, MassMeasurementUnit.Kilogram),
                Description.Create("my very very cool stuff")
            );

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_all_values_match()
        {
            var first = ListingDetails.Create
            (
                Title.Create("my stuff"),
                MaterialType.Electronics,
                Weight.Create(2.5F, MassMeasurementUnit.Kilogram),
                Description.Create("my very very cool stuff")
            );
            var second = ListingDetails.Create
            (
                Title.Create("my stuff"),
                MaterialType.Electronics,
                Weight.Create(2.5F, MassMeasurementUnit.Kilogram),
                Description.Create("my very very cool stuff")
            );

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_any_value_doesnt_match()
        {
            var first = ListingDetails.Create
            (
                Title.Create("my stuff"),
                MaterialType.Electronics,
                Weight.Create(2.5F, MassMeasurementUnit.Kilogram),
                Description.Create("my very very cool stuff")
            );
            var second = ListingDetails.Create
            (
                Title.Create("my stufffffff"),
                MaterialType.Electronics,
                Weight.Create(2.5F, MassMeasurementUnit.Kilogram),
                Description.Create("my very very cool stuff")
            );

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
