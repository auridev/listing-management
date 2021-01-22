using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
{
    public sealed class MonetaryValue : IEquatable<MonetaryValue>
    {
        public decimal Value { get; }

        public CurrencyCode CurrencyCode { get; }

        private MonetaryValue() { }
        private MonetaryValue(decimal value, CurrencyCode currencyCode)
        {
            Value = value;
            CurrencyCode = currencyCode;
        }

        public static Either<Error, MonetaryValue> Create(decimal value, string currencyCode)
        {
            Either<Error, decimal> eitherValue = EnsureValidDecimal(value);
            Either<Error, CurrencyCode> eitherCurrencyCode = CurrencyCode.Create(currencyCode);

            Either<Error, (decimal value, CurrencyCode currencyCode)> combined =
                from v in eitherValue
                from cc in eitherCurrencyCode
                select (v, cc);

            return
                combined.Map(
                    combined =>
                        new MonetaryValue(combined.value, combined.currencyCode));
        }

        private static Either<Error, decimal> EnsureValidDecimal(decimal value)
            =>
                (value > 0)
                    ? Right(value)
                    : Left(Invalid<decimal>("invalid monetary value"));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<MonetaryValue>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] MonetaryValue other)
            =>
                ValueObjectComparer<MonetaryValue>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<MonetaryValue>.Instance.GetHashCode();

        public static bool operator ==(MonetaryValue left, MonetaryValue right)
            =>
                ValueObjectComparer<MonetaryValue>.Instance.Equals(left, right);

        public static bool operator !=(MonetaryValue left, MonetaryValue right)
            =>
                !(left == right);
    }
}
