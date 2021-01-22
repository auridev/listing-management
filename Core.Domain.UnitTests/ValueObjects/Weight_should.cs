using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Weight_should
    {
        [Fact]
        public void have_Value_property()
        {
            Weight
                .Create(20F, "lb")
                .Right(weight => weight.Value.Should().Be(20F))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_unit_property()
        {
            Weight
                .Create(1F, "kg")
                .Right(weight => weight.Unit.Should().Be(MassMeasurementUnit.Kilogram))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid(float value)
        {
            Either<Error, Weight> eitherWeight = Weight.Create(value, "lb");

            eitherWeight.IsLeft.Should().BeTrue();
            eitherWeight
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { -1 },
            new object[] { 0 },
            new object[] { -3_000.5F },
            new object[] { default }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_and_Units_match()
        {
            var first = Weight.Create(58F, "kg");
            var second = Weight.Create(58F, "kg");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_and_Units_match()
        {
            var first = (object)Weight.Create(58F, "kg");
            var second = (object)Weight.Create(58F, "kg");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_and_Units_match()
        {
            var first = Weight.Create(0.555F, "lb");
            var second = Weight.Create(0.555F, "lb");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(ValuesAndUnits))]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_either_Values_or_Units_dont_match
        (
            float firstValue,
            string firstUnit,
            float secondValue,
            string secondUnit
        )
        {
            var first = Weight.Create(firstValue, firstUnit);
            var second = Weight.Create(secondValue, secondUnit);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        public static IEnumerable<object[]> ValuesAndUnits => new List<object[]>
        {
            new object[] { 1.01F, "lb", 2.02F, "kg" },
            new object[] { 1.01F, "lb", 1.01F, "kg" },
            new object[] { 1.01F, "kg", 2.02F, "kg" }
        };
    }
}
