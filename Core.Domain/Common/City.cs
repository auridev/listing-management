using Core.Domain.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class City : IEquatable<City>
    {
        public TrimmedString Name { get; }

        private City() { }

        private City(TrimmedString name)
        {
            Name = name;
        }
        public static City Create(string name)
        {
            name = name.CapitalizeWords();

            return new City((TrimmedString)name);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<City>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] City other)
            => ValueObjectComparer<City>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<City>.Instance.GetHashCode();

        public static bool operator ==(City left, City right)
            => ValueObjectComparer<City>.Instance.Equals(left, right);

        public static bool operator !=(City left, City right)
            => !(left == right);
    }
}
