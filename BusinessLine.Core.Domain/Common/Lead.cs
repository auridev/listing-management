using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class Lead
    {
        public Owner Owner { get; }
        public DateTimeOffset CreatedDate { get; }

        private Lead() { }
        private Lead(Owner owner, DateTimeOffset createdDate)
        {
            if (createdDate == default)
                throw new ArgumentNullException(nameof(createdDate));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            Owner = owner;
            CreatedDate = createdDate;
        }

        public static Lead Create(Owner owner, DateTimeOffset createdDate)
            => new Lead(owner, createdDate);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Lead>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Lead other)
            => ValueObjectComparer<Lead>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Lead>.Instance.GetHashCode();

        public static bool operator ==(Lead left, Lead right)
            => ValueObjectComparer<Lead>.Instance.Equals(left, right);

        public static bool operator !=(Lead left, Lead right)
            => !(left == right);
    }
}
