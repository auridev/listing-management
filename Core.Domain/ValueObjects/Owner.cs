using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class Owner : IEquatable<Owner>
    {
        public Guid UserId { get; }

        private Owner() { }
        private Owner(Guid userId)
        {
            UserId = userId;
        }

        public static Owner Create(Guid userId)
        {
            if (userId == default)
                throw new ArgumentException(nameof(userId));

            return new Owner(userId);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Owner>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Owner other)
            => ValueObjectComparer<Owner>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Owner>.Instance.GetHashCode();

        public static bool operator ==(Owner left, Owner right)
            => ValueObjectComparer<Owner>.Instance.Equals(left, right);

        public static bool operator !=(Owner left, Owner right)
            => !(left == right);
    }
}