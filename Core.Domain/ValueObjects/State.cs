using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public class State : IEquatable<State>
    {
        public virtual TrimmedString Name { get; }

        protected State() { }

        private State(TrimmedString name)
        {
            Name = name;
        }

        public static Either<Error, State> Create(string name)
            =>
                EnsureNonEmpty(name)
                    .Bind(name => CapitalizeAllWords(name))
                    .Bind(value => ConvertToTrimmedString(value))
                    .Bind(value => CreateState(value));

        private static Either<Error, TrimmedString> ConvertToTrimmedString(string value)
            =>
                TrimmedString.Create(value);

        private static Either<Error, State> CreateState(Either<Error, TrimmedString> value)
            =>
                value.Map(value => new State(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<State>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] State other)
            =>
                ValueObjectComparer<State>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<State>.Instance.GetHashCode();

        public static bool operator ==(State left, State right)
            =>
                ValueObjectComparer<State>.Instance.Equals(left, right);

        public static bool operator !=(State left, State right)
            =>
                !(left == right);
    }
}
