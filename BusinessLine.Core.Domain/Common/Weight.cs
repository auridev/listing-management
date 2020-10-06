using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
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

        public static Weight Create(float value, MassMeasurementUnit unit)
        {
            if (value <= 0)
                throw new ArgumentException(nameof(value));

            return new Weight(value, unit);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Weight>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Weight other)
            => ValueObjectComparer<Weight>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Weight>.Instance.GetHashCode();

        public static bool operator ==(Weight left, Weight right)
            => ValueObjectComparer<Weight>.Instance.Equals(left, right);

        public static bool operator !=(Weight left, Weight right)
            => !(left == right);
    }
}
