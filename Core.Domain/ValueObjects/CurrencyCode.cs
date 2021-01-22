using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class CurrencyCode : IEquatable<CurrencyCode>
    {
        public static CurrencyCode EUR
            =>
                new CurrencyCode("EUR");

        public static CurrencyCode USD
            =>
                new CurrencyCode("USD");

        public static CurrencyCode PLN
            =>
                new CurrencyCode("PLN");

        public static CurrencyCode GBP
            =>
                new CurrencyCode("GBP");

        private static readonly int _exactLength = 3;

        public string Value { get; }

        private CurrencyCode() { }

        private CurrencyCode(string value)
        {
            Value = value;
        }

        public static Either<Error, CurrencyCode> Create(string value)
            =>
                EnsureNonEmpty(value)
                    .Bind(value => Trim(value))
                    .Bind(value => ConvertToUpper(value))
                    .Map(value => EnsureRequiredLength(value, _exactLength))
                    .Bind(value => CreateCode(value));

        private static Either<Error, CurrencyCode> CreateCode(Either<Error, string> input)
            =>
                input.Map(value => new CurrencyCode(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<CurrencyCode>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] CurrencyCode other)
            =>
                ValueObjectComparer<CurrencyCode>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<CurrencyCode>.Instance.GetHashCode();

        public static bool operator ==(CurrencyCode left, CurrencyCode right)
            =>
                ValueObjectComparer<CurrencyCode>.Instance.Equals(left, right);

        public static bool operator !=(CurrencyCode left, CurrencyCode right)
            =>
                !(left == right);
    }
}
