using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.StringHelpers;

namespace Core.Domain.ValueObjects
{
    public sealed class Alpha2Code : IEquatable<Alpha2Code>
    {
        private readonly static int _exactLength = 2;
        public string Value { get; }

        private Alpha2Code() { }

        private Alpha2Code(string value)
        {
            Value = value;
        }

        public static Either<Error, Alpha2Code> Create(string value)
            =>
                EnsureNonEmpty(value)
                    .Bind(value => Trim(value))
                    .Bind(value => ConvertToUpper(value))
                    .Map(value => EnsureRequiredLength(value, _exactLength))
                    .Bind(value => CreateCode(value));

        private static Either<Error, Alpha2Code> CreateCode(Either<Error, string> input)
            =>
                input.Map(value => new Alpha2Code(value));

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Alpha2Code>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Alpha2Code other)
            => ValueObjectComparer<Alpha2Code>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Alpha2Code>.Instance.GetHashCode();

        public static bool operator ==(Alpha2Code left, Alpha2Code right)
            => ValueObjectComparer<Alpha2Code>.Instance.Equals(left, right);

        public static bool operator !=(Alpha2Code left, Alpha2Code right)
            => !(left == right);
    }
}
