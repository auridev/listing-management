using Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Weight_should
    {
        [Fact]
        public void have_Value_property()
        {
            var weight = Weight.Create(20F, MassMeasurementUnit.Pound);
            weight.Value.Should().Be(20F);
        }

        [Fact]
        public void have_unit_property()
        {
            var weight = Weight.Create(1F, MassMeasurementUnit.Kilogram);
            weight.Unit.Should().Be(MassMeasurementUnit.Kilogram);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void throw_an_exception_during_creation_if_value_is_not_valid(float value)
        {
            Action create = () =>Weight.Create(value, MassMeasurementUnit.Pound);

            create.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { -1 },
            new object[] { 0 },
            new object[] { -3_000.5F },
            new object[] { default }
        };

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_and_Units_match()
        {
            var first = Weight.Create(58F, MassMeasurementUnit.Kilogram);
            var second = Weight.Create(58F, MassMeasurementUnit.Kilogram);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_and_Units_match()
        {
            var first = Weight.Create(0.555F, MassMeasurementUnit.Pound);
            var second = Weight.Create(0.555F, MassMeasurementUnit.Pound);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(ValuesAndUnits))]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_either_Values_or_Units_dont_match
        (
            float firstValue,
            MassMeasurementUnit firstUnit,
            float secondValue,
            MassMeasurementUnit secondUnit
        )
        {
            var first = Weight.Create(firstValue, firstUnit);
            var second = Weight.Create(secondValue, secondUnit);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        public static IEnumerable<object[]> ValuesAndUnits => new List<object[]>
        {
            new object[] { 1.01F, MassMeasurementUnit.Pound, 2.02F, MassMeasurementUnit.Kilogram },
            new object[] { 1.01F, MassMeasurementUnit.Pound, 1.01F, MassMeasurementUnit.Kilogram },
            new object[] { 1.01F, MassMeasurementUnit.Kilogram, 2.02F, MassMeasurementUnit.Kilogram }
        };
    }
}
