using Core.Domain.Common;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class DistanceMeasurementUnit_should
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void have_predefined_options(DistanceMeasurementUnit unit, int expectedId, string expectedName, string expectedSymbol)
        {
            unit.Id.Should().Be(expectedId);
            unit.Name.Should().Be(expectedName);
            unit.Symbol.Should().Be(expectedSymbol);
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { DistanceMeasurementUnit.Kilometer,  10, "Kilometer", "km"},
            new object[] { DistanceMeasurementUnit.Mile,  20, "Mile", "m"}
        };

        [Theory]
        [MemberData(nameof(UnitsBySymbols))]
        public void return_expected_unit_by_symbol(string symbol, DistanceMeasurementUnit expectedUnit)
        {
            DistanceMeasurementUnit unit = DistanceMeasurementUnit.BySymbol(symbol);

            unit.Should().Be(expectedUnit);
        }

        public static IEnumerable<object[]> UnitsBySymbols => new List<object[]>
        {
            new object[] { "km", DistanceMeasurementUnit.Kilometer },
            new object[] { "m", DistanceMeasurementUnit.Mile }
        };

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_predefined_values_match()
        {
            var first = DistanceMeasurementUnit.Kilometer;
            var second = DistanceMeasurementUnit.Kilometer;

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_predefined_values_match()
        {
            var first = DistanceMeasurementUnit.Mile;
            var second = DistanceMeasurementUnit.Mile;

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_predefined_values_dont_match()
        {
            var first = DistanceMeasurementUnit.Mile;
            var second = DistanceMeasurementUnit.Kilometer;

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
