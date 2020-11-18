using Core.Domain.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class Currency : IEquatable<Currency>
    {
        public static Currency Euro { get; } = new Currency(
            CurrencyCode.Create("EUR"), 
            "€", 
            "Euro");

        public static Currency USDollar { get; } = new Currency(
            CurrencyCode.Create("USD"),
            "$",
            "US Dollar");

        public CurrencyCode Code { get; }
        public string Symbol { get; }
        public string Name { get; }

        private Currency() { }

        private Currency(CurrencyCode code, string symbol, string name)
        {
            Code = code;
            Symbol = symbol;
            Name = name;
        }

        public static Currency Create(CurrencyCode code, string symbol, string name)
        {
            if (code == null)
                throw new ArgumentException(nameof(code));
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            symbol = symbol.Trim();
            name = name.Trim().CapitalizeWords();

            return new Currency(code, symbol, name);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Currency>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Currency other)
            => ValueObjectComparer<Currency>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Currency>.Instance.GetHashCode();

        public static bool operator ==(Currency left, Currency right)
            => ValueObjectComparer<Currency>.Instance.Equals(left, right);

        public static bool operator !=(Currency left, Currency right)
            => !(left == right);

    }
}
