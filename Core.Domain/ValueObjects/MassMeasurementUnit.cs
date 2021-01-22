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
    public sealed class MassMeasurementUnit : IEquatable<MassMeasurementUnit>
    {
        public static readonly MassMeasurementUnit Kilogram = new MassMeasurementUnit(10, "Kilogram", "kg");
        public static readonly MassMeasurementUnit Pound = new MassMeasurementUnit(20, "Pound", "lb");
        private static readonly Dictionary<string, MassMeasurementUnit> _units =
            new Dictionary<string, MassMeasurementUnit>
            {
                        { Kilogram.Symbol, Kilogram },
                        { Pound.Symbol, Pound }
            };

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

        public static Either<Error, MassMeasurementUnit> BySymbol(string symbol)
            =>
                (_units.ContainsKey(symbol))
                    ? Right(_units[symbol])
                    : Left(Invalid<MassMeasurementUnit>("unknown mass measurement unit"));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<MassMeasurementUnit>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] MassMeasurementUnit other)
            =>
                ValueObjectComparer<MassMeasurementUnit>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<MassMeasurementUnit>.Instance.GetHashCode();

        public static bool operator ==(MassMeasurementUnit left, MassMeasurementUnit right)
            =>
                ValueObjectComparer<MassMeasurementUnit>.Instance.Equals(left, right);

        public static bool operator !=(MassMeasurementUnit left, MassMeasurementUnit right)
            =>
                !(left == right);
    }
}
