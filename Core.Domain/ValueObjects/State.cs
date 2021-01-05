using Common.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

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
        public static State Create(string name)
        {
            name = name.CapitalizeWords();

            return TrimmedString
              .Create(name)
              .Match(
                  success => new State(success),
                  error => null);
        }
        // => new State((TrimmedString)name.CapitalizeWords());

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<State>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] State other)
            => ValueObjectComparer<State>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<State>.Instance.GetHashCode();

        public static bool operator ==(State left, State right)
            => ValueObjectComparer<State>.Instance.Equals(left, right);

        public static bool operator !=(State left, State right)
            => !(left == right);
    }
}
