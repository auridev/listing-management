using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Domain.Offers
{
    public abstract class Offer : IEquatable<Offer>
    {
        public Guid Id { get; }
        public Owner Owner { get; }
        public MonetaryValue MonetaryValue { get; }
        public DateTimeOffset CreatedDate { get; }

        protected Offer() { }

        public Offer(Guid id,
            Owner owner,
            MonetaryValue monetaryValue,
            DateTimeOffset createdDate)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (monetaryValue == null)
                throw new ArgumentNullException(nameof(monetaryValue));
            if (createdDate == default)
                throw new ArgumentNullException(nameof(createdDate));

            Id = id;
            Owner = owner;
            MonetaryValue = monetaryValue;
            CreatedDate = createdDate;
        }

        public bool Equals([AllowNull] Offer other)
        {
            if (GetType() != other.GetType())
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id.IsDefault() || other.Id.IsDefault())
                return false;

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Offer other))
                return false;

            return Equals(other);
        }

        public static bool operator ==(Offer a, Offer b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Offer a, Offer b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}
