using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class Currency : IEquatable<Currency>
    {
        public static Currency Euro
            =>
                new Currency(CurrencyCode.EUR, "€", "Euro");

        public static Currency USDollar
            =>
                new Currency(CurrencyCode.USD, "$", "US Dollar");

        public static Currency PolishZloty
            =>
                new Currency(CurrencyCode.PLN, "zł", "PZloty");

        public static Currency PoundSterling
            =>
                new Currency(CurrencyCode.GBP, "£", "Pound Sterling");

        public CurrencyCode Code { get; }
        public string Symbol { get; }
        public string Name { get; }

        private Currency() { }

        private Currency(
            CurrencyCode code,
            string symbol,
            string name)
        {
            Code = code;
            Symbol = symbol;
            Name = name;
        }

        public static Either<Error, Currency> Create(string currencyCode, string symbol, string name)
        {
            Either<Error, CurrencyCode> eitherCurrencyCode = CurrencyCode.Create(currencyCode);
            Either<Error, string> eitherSymbol =
                EnsureNonEmpty(symbol)
                    .Bind(value => Trim(value));
            Either<Error, string> eitherName =
                EnsureNonEmpty(name)
                    .Bind(value => Trim(value));

            Either<Error, (CurrencyCode currencyCode, string symbol, string name)> combined =
                from cc in eitherCurrencyCode
                from s in eitherSymbol
                from n in eitherName
                select (cc, s, n);

            return
                combined.Map(
                    combined =>
                        new Currency(
                            combined.currencyCode,
                            combined.symbol,
                            combined.name));
        }

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Currency>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Currency other)
            =>
                ValueObjectComparer<Currency>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Currency>.Instance.GetHashCode();

        public static bool operator ==(Currency left, Currency right)
            =>
                ValueObjectComparer<Currency>.Instance.Equals(left, right);

        public static bool operator !=(Currency left, Currency right)
            =>
                !(left == right);
    }
}
