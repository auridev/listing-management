using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    // represents the fact of a particular user (Owner) being interested in the details of a particular parent entitity
    public sealed class Lead
    {
        public Owner UserInterested { get; }
        public DateTimeOffset DetailsSeenOn { get; }

        private Lead() { }
        private Lead(Owner userInterested, DateTimeOffset detailsSeenOn)
        {
            if (detailsSeenOn == default)
                throw new ArgumentNullException(nameof(detailsSeenOn));
            if (userInterested == null)
                throw new ArgumentNullException(nameof(userInterested));

            UserInterested = userInterested;
            DetailsSeenOn = detailsSeenOn;
        }

        public static Lead Create(Owner userInterested, DateTimeOffset detailsSeenOn)
            => new Lead(userInterested, detailsSeenOn);

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
