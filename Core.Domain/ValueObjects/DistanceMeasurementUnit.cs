using Common.Helpers;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
{
    public sealed class DistanceMeasurementUnit : IEquatable<DistanceMeasurementUnit>
    {
        public static readonly DistanceMeasurementUnit Kilometer = new DistanceMeasurementUnit(10, "Kilometer", "km");
        public static readonly DistanceMeasurementUnit Mile = new DistanceMeasurementUnit(20, "Mile", "m");
        private static readonly Dictionary<string, DistanceMeasurementUnit> _units =
            new Dictionary<string, DistanceMeasurementUnit>
            {
                { Kilometer.Symbol, Kilometer },
                { Mile.Symbol, Mile }
            };

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

        public static Either<Error, DistanceMeasurementUnit> BySymbol(string symbol)
            =>
                (_units.ContainsKey(symbol))
                    ? Right(_units[symbol])
                    : Left(Invalid<DistanceMeasurementUnit>("unknown distance measurement unit"));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<DistanceMeasurementUnit>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] DistanceMeasurementUnit other)
            =>
                ValueObjectComparer<DistanceMeasurementUnit>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<DistanceMeasurementUnit>.Instance.GetHashCode();

        public static bool operator ==(DistanceMeasurementUnit left, DistanceMeasurementUnit right)
            =>
                ValueObjectComparer<DistanceMeasurementUnit>.Instance.Equals(left, right);

        public static bool operator !=(DistanceMeasurementUnit left, DistanceMeasurementUnit right)
            =>
                !(left == right);
    }
}
