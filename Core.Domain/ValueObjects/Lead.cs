using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

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
            UserInterested = userInterested;
            DetailsSeenOn = detailsSeenOn;
        }

        public static Either<Error, Lead> Create(Guid interestedUserId, DateTimeOffset detailsSeenOn)
        {
            Either<Error, Owner> eitherOwner = Owner.Create(interestedUserId);
            Either<Error, DateTimeOffset> eitherSeenOn = EnsureNonDefault(detailsSeenOn);

            Either<Error, (Owner userInterested, DateTimeOffset detailsSeenOn)> combined =
                from o in eitherOwner
                from s in eitherSeenOn
                select (o, s);

            return
                combined.Map(
                    combined =>
                        new Lead(combined.userInterested, combined.detailsSeenOn));
        }

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Lead>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Lead other)
            =>
                ValueObjectComparer<Lead>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Lead>.Instance.GetHashCode();

        public static bool operator ==(Lead left, Lead right)
            =>
                ValueObjectComparer<Lead>.Instance.Equals(left, right);

        public static bool operator !=(Lead left, Lead right)
            =>
                !(left == right);
    }
}
