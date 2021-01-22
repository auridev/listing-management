using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class TrimmedString : IEquatable<TrimmedString>
    {
        public string Value { get; }

        private TrimmedString() { }

        private TrimmedString(string value)
        {
            Value = value;
        }

        public static Either<Error, TrimmedString> Create(string value)
            =>
                EnsureNonEmpty(value)
                    .Bind(value => Trim(value))
                    .Bind(value => CreateTrimmedString(value));

        private static Either<Error, TrimmedString> CreateTrimmedString(Either<Error, string> input)
            =>
                input.Map(value => new TrimmedString(value));

        public override string ToString()
            =>
                Value;

        public static implicit operator string(TrimmedString trimmedString)
            =>
                trimmedString.Value;

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<TrimmedString>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] TrimmedString other)
            =>
                ValueObjectComparer<TrimmedString>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<TrimmedString>.Instance.GetHashCode();

        public static bool operator ==(TrimmedString left, TrimmedString right)
            =>
                ValueObjectComparer<TrimmedString>.Instance.Equals(left, right);

        public static bool operator !=(TrimmedString left, TrimmedString right)
            =>
                !(left == right);
    }
}
