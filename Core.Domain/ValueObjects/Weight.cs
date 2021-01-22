using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
{
    public sealed class Weight : IEquatable<Weight>
    {
        public float Value { get; }
        public MassMeasurementUnit Unit { get; }

        private Weight() { }

        private Weight(float value, MassMeasurementUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public static Either<Error, Weight> Create(float value, string massUnit)
        {
            Either<Error, float> eitherWeight = EnsureValidWeight(value);
            Either<Error, MassMeasurementUnit> eitherMassUnit = MassMeasurementUnit.BySymbol(massUnit);

            Either<Error, (float value, MassMeasurementUnit unit)> combined =
                from w in eitherWeight
                from m in eitherMassUnit
                select (w, m);

            return
                combined.Map(
                    combined =>
                        new Weight(combined.value, combined.unit));
        }

        private static Either<Error, float> EnsureValidWeight(float value)
            =>
                (value > 0)
                    ? Right(value)
                    : Left(Invalid<float>("weight needs to be greater than 0"));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Weight>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Weight other)
            =>
                ValueObjectComparer<Weight>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Weight>.Instance.GetHashCode();

        public static bool operator ==(Weight left, Weight right)
            =>
                ValueObjectComparer<Weight>.Instance.Equals(left, right);

        public static bool operator !=(Weight left, Weight right)
            =>
                !(left == right);
    }
}
