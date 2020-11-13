using Core.Domain.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class Country : IEquatable<Country>
    {
        public string Name { get; }
        public Alpha2Code Alpha2Code { get; }
        public Alpha3Code Alpha3Code { get; }
        public Currency Currency { get;  }

        private Country() { }

        private Country(string name, Alpha2Code alpha2Code, Alpha3Code alpha3Code, Currency currency)
        {
            Name = name;
            Alpha2Code = alpha2Code;
            Alpha3Code = alpha3Code;
            Currency = currency;
        }

        public static Country Create(string name, Alpha2Code alpha2Code, Alpha3Code alpha3Code, Currency currency)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));
            if (alpha2Code == null)
                throw new ArgumentException(nameof(alpha2Code));
            if (alpha3Code == null)
                throw new ArgumentException(nameof(alpha3Code));
            if (currency == null)
                throw new ArgumentException(nameof(currency));

            name = name.Trim().CapitalizeWords();

            return new Country(name, alpha2Code, alpha3Code, currency);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Country>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Country other)
            => ValueObjectComparer<Country>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Country>.Instance.GetHashCode();

        public static bool operator ==(Country left, Country right)
            => ValueObjectComparer<Country>.Instance.Equals(left, right);

        public static bool operator !=(Country left, Country right)
            => !(left == right);
    }
}
