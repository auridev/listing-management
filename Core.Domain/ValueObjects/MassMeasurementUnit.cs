using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class MassMeasurementUnit : IEquatable<MassMeasurementUnit>
    {
        public static readonly MassMeasurementUnit Kilogram = new MassMeasurementUnit(10, "Kilogram", "kg");
        public static readonly MassMeasurementUnit Pound = new MassMeasurementUnit(20, "Pound", "lb");

        public int Id { get; }
        public string Name { get; }
        public string Symbol { get; }

        private MassMeasurementUnit() { }
        private MassMeasurementUnit(int id, string name, string symbol)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
        }
        public static MassMeasurementUnit BySymbol(string symbol)
        {
            return symbol == "lb" ? Pound : Kilogram;
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<MassMeasurementUnit>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] MassMeasurementUnit other)
            => ValueObjectComparer<MassMeasurementUnit>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<MassMeasurementUnit>.Instance.GetHashCode();

        public static bool operator ==(MassMeasurementUnit left, MassMeasurementUnit right)
            => ValueObjectComparer<MassMeasurementUnit>.Instance.Equals(left, right);

        public static bool operator !=(MassMeasurementUnit left, MassMeasurementUnit right)
            => !(left == right);
    }
}
