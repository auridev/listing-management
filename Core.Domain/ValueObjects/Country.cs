using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class Country : IEquatable<Country>
    {
        public string Name { get; }
        public Alpha2Code Alpha2Code { get; }
        public Alpha3Code Alpha3Code { get; }
        public Currency Currency { get; }

        private Country() { }

        private Country(
            string name,
            Alpha2Code alpha2Code,
            Alpha3Code alpha3Code,
            Currency currency)
        {
            Name = name;
            Alpha2Code = alpha2Code;
            Alpha3Code = alpha3Code;
            Currency = currency;
        }

        public static Either<Error, Country> Create(
            string name,
            string alpha2Code,
            string alpha3Code,
            string currencyCode,
            string currencySymbol,
            string currencyName)
        {
            Either<Error, TrimmedString> eitherName =
                EnsureNonEmpty(name)
                    .Bind(name => CapitalizeAllWords(name))
                    .Bind(name => Trim(name))
                    .Bind(name => TrimmedString.Create(name));
            Either<Error, Alpha2Code> eitherAlpha2Code = Alpha2Code.Create(alpha2Code);
            Either<Error, Alpha3Code> eitherAlpha3Code = Alpha3Code.Create(alpha3Code);
            Either<Error, Currency> eitherCurrency = Currency.Create(currencyCode, currencySymbol, currencyName);

            Either<Error, (TrimmedString name, Alpha2Code alpha2Code, Alpha3Code alpha3Code, Currency currency)> combined =
                from n in eitherName
                from a2c in eitherAlpha2Code
                from a3c in eitherAlpha3Code
                from c in eitherCurrency
                select (n, a2c, a3c, c);

            return
                combined.Map(
                    combined =>
                        new Country(
                            combined.name,
                            combined.alpha2Code,
                            combined.alpha3Code,
                            combined.currency));
        }

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Country>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Country other)
            =>
                ValueObjectComparer<Country>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Country>.Instance.GetHashCode();

        public static bool operator ==(Country left, Country right)
            =>
                ValueObjectComparer<Country>.Instance.Equals(left, right);

        public static bool operator !=(Country left, Country right)
            =>
                !(left == right);
    }
}
