using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.StringHelpers;

namespace Core.Domain.ValueObjects
{
    public sealed class Alpha3Code : IEquatable<Alpha3Code>
    {
        private readonly static int _exactLength = 3;
        public string Value { get; }

        private Alpha3Code() { }

        private Alpha3Code(string value)
        {
            Value = value;
        }

        public static Either<Error, Alpha3Code> Create(string value)
            =>
                EnsureNonEmpty(value)
                    .Bind(value => Trim(value))
                    .Bind(value => ConvertToUpper(value))
                    .Map(value => EnsureRequiredLength(value, _exactLength))
                    .Bind(value => CreateCode(value));

        private static Either<Error, Alpha3Code> CreateCode(Either<Error, string> input)
            =>
                input.Map(value => new Alpha3Code(value));

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Alpha3Code>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Alpha3Code other)
            => ValueObjectComparer<Alpha3Code>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Alpha3Code>.Instance.GetHashCode();

        public static bool operator ==(Alpha3Code left, Alpha3Code right)
            => ValueObjectComparer<Alpha3Code>.Instance.Equals(left, right);

        public static bool operator !=(Alpha3Code left, Alpha3Code right)
            => !(left == right);
    }
}
