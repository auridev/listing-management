using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class MassMeasurementUnit_should
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void have_predefined_options(MassMeasurementUnit unit, int expectedId, string expectedName, string expectedSymbol)
        {
            unit.Id.Should().Be(expectedId);
            unit.Name.Should().Be(expectedName);
            unit.Symbol.Should().Be(expectedSymbol);
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { MassMeasurementUnit.Kilogram,  10, "Kilogram", "kg"},
            new object[] { MassMeasurementUnit.Pound,  20, "Pound", "lb"}
        };

        [Theory]
        [MemberData(nameof(UnitsBySymbols))]
        public void return_expected_unit_by_symbol(string symbol, MassMeasurementUnit expectedUnit)
        {
            Either<Error, MassMeasurementUnit> eitherUnit = MassMeasurementUnit.BySymbol(symbol);

            eitherUnit
                .Right(unit => unit.Should().Be(expectedUnit))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> UnitsBySymbols => new List<object[]>
        {
            new object[] { "kg", MassMeasurementUnit.Kilogram },
            new object[] { "lb", MassMeasurementUnit.Pound }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_predefined_values_match()
        {
            var first = MassMeasurementUnit.Kilogram;
            var second = MassMeasurementUnit.Kilogram;

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_predefined_values_match()
        {
            var first = (object)MassMeasurementUnit.Kilogram;
            var second = (object)MassMeasurementUnit.Kilogram;

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_predefined_values_match()
        {
            var first = MassMeasurementUnit.Pound;
            var second = MassMeasurementUnit.Pound;

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_predefined_values_dont_match()
        {
            var first = MassMeasurementUnit.Pound;
            var second = MassMeasurementUnit.Kilogram;

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
