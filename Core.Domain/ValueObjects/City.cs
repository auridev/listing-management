using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class City : IEquatable<City>
    {
        public TrimmedString Name { get; }

        private City() { }

        private City(TrimmedString name)
        {
            Name = name;
        }
        public static Either<Error, City> Create(string name)
            =>
                EnsureNonEmpty(name)
                    .Bind(name => CapitalizeAllWords(name))
                    .Bind(cityName => TrimmedString.Create(cityName))
                    .Bind(cityName => CreateCity(cityName));

        private static Either<Error, City> CreateCity(Either<Error, TrimmedString> input)
            =>
                input.Map(value => new City(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<City>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] City other)
            =>
                ValueObjectComparer<City>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<City>.Instance.GetHashCode();

        public static bool operator ==(City left, City right)
            =>
                ValueObjectComparer<City>.Instance.Equals(left, right);

        public static bool operator !=(City left, City right)
            =>
                !(left == right);
    }
}
