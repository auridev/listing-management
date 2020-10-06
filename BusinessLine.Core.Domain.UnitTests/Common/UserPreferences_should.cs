using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class UserPreferences_should
    {
        private readonly UserPreferences _sut;

        public UserPreferences_should()
        {
            _sut = UserPreferences.Create(
                DistanceMeasurementUnit.Kilometer,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("aaa"));
        }

        [Fact]
        public void have_DistanceUnit_property()
        {
            _sut.DistanceUnit.Should().Be(DistanceMeasurementUnit.Kilometer);
        }

        [Fact]
        public void have_MassUnit_property()
        {
            _sut.MassUnit.Should().Be(MassMeasurementUnit.Pound);
        }

        [Fact]
        public void have_CurrencyCode_property()
        {
            _sut.CurrencyCode.Should().Be(CurrencyCode.Create("aaa"));
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_all_values_match()
        {
            var first = UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("usd"));
            var second = UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("usd"));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_all_values_match()
        {
            var first = (object)UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("usd"));
            var second = (object)UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("usd"));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_all_values_match()
        {
            var first = UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("usd"));
            var second = UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("usd"));

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_any_value_doesnt_match()
        {
            var first = UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("usd"));
            var second = UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Kilogram,
                CurrencyCode.Create("usd"));

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
