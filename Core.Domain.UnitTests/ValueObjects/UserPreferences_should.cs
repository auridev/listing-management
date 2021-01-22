using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class UserPreferences_should
    {
        private readonly Either<Error, UserPreferences> _sut = UserPreferences.Create(
                DistanceMeasurementUnit.Kilometer.Symbol,
                MassMeasurementUnit.Pound.Symbol,
                "aaa");
               
        [Fact]
        public void have_DistanceUnit_property()
        {
            _sut
                .Right(preferences => preferences.DistanceUnit.Should().Be(DistanceMeasurementUnit.Kilometer))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_MassUnit_property()
        {
            _sut
                .Right(preferences => preferences.MassUnit.Should().Be(MassMeasurementUnit.Pound))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_CurrencyCode_property()
        {
            _sut
                .Right(preferences => preferences.CurrencyCode.Value.Should().Be("AAA"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_all_values_match()
        {
            var first = UserPreferences.Create(
                DistanceMeasurementUnit.Mile.Symbol,
                MassMeasurementUnit.Pound.Symbol,
                "usd");
            var second = UserPreferences.Create(
                DistanceMeasurementUnit.Mile.Symbol,
                MassMeasurementUnit.Pound.Symbol,
                "usd");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_all_values_match()
        {
            var first = (object)UserPreferences.Create(
                DistanceMeasurementUnit.Mile.Symbol,
                MassMeasurementUnit.Pound.Symbol,
                "usd");
            var second = (object)UserPreferences.Create(
                DistanceMeasurementUnit.Mile.Symbol,
                MassMeasurementUnit.Pound.Symbol,
                "usd");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_all_values_match()
        {
            var first = UserPreferences.Create(
                DistanceMeasurementUnit.Mile.Symbol,
                MassMeasurementUnit.Pound.Symbol,
                "usd");
            var second = UserPreferences.Create(
                DistanceMeasurementUnit.Mile.Symbol,
                MassMeasurementUnit.Pound.Symbol,
                "usd");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_any_value_doesnt_match()
        {
            var first = UserPreferences.Create(
                DistanceMeasurementUnit.Mile.Symbol,
                MassMeasurementUnit.Pound.Symbol,
                "usd");
            var second = UserPreferences.Create(
                DistanceMeasurementUnit.Mile.Symbol,
                MassMeasurementUnit.Kilogram.Symbol,
                "usd");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
