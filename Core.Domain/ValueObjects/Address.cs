using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class Address : IEquatable<Address>
    {
        public TrimmedString Value { get; }

        private Address() { }

        private Address(TrimmedString value)
        {
            Value = value;
        }

        public static Either<Error, Address> Create(string value)
            =>
                ConvertInputValueToTrimmedString(value)
                    .Bind(value => CreateAddress(value));

        private static Either<Error, TrimmedString> ConvertInputValueToTrimmedString(string value)
            =>
                TrimmedString.Create(value);

        private static Either<Error, Address> CreateAddress(Either<Error, TrimmedString> input)
            =>
                input.Map(value => new Address(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Address>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Address other)
            =>
                ValueObjectComparer<Address>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Address>.Instance.GetHashCode();

        public static bool operator ==(Address left, Address right)
            =>
                ValueObjectComparer<Address>.Instance.Equals(left, right);

        public static bool operator !=(Address left, Address right)
            =>
                !(left == right);
    }
}
