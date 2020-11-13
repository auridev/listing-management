using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class DistanceMeasurementUnit : IEquatable<DistanceMeasurementUnit>
    {
        public static readonly DistanceMeasurementUnit Kilometer = new DistanceMeasurementUnit(10, "Kilometer", "km");
        public static readonly DistanceMeasurementUnit Mile = new DistanceMeasurementUnit(20, "Mile", "m");

        public int Id { get; }
        public string Name { get; }
        public string Symbol { get; }

        private DistanceMeasurementUnit() { }

        private DistanceMeasurementUnit(int id, string name, string symbol)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
        }

        public static DistanceMeasurementUnit BySymbol(string symbol)
        {
            return symbol == "m" ? Mile : Kilometer;
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<DistanceMeasurementUnit>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] DistanceMeasurementUnit other)
            => ValueObjectComparer<DistanceMeasurementUnit>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<DistanceMeasurementUnit>.Instance.GetHashCode();

        public static bool operator ==(DistanceMeasurementUnit left, DistanceMeasurementUnit right)
            => ValueObjectComparer<DistanceMeasurementUnit>.Instance.Equals(left, right);

        public static bool operator !=(DistanceMeasurementUnit left, DistanceMeasurementUnit right)
            => !(left == right);
    }
}
