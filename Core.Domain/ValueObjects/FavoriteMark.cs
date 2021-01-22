using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class FavoriteMark
    {
        public Owner FavoredBy { get; }
        public DateTimeOffset MarkedAsFavoriteOn { get; }

        private FavoriteMark() { }
        private FavoriteMark(Owner favoredBy, DateTimeOffset markedAsFavoriteOn)
        {
            FavoredBy = favoredBy;
            MarkedAsFavoriteOn = markedAsFavoriteOn;
        }

        public static Either<Error, FavoriteMark> Create(Guid favoredBy, DateTimeOffset markedAsFavoriteOn)
        {
            Either<Error, Owner> eitherFavoredBy = Owner.Create(favoredBy);
            Either<Error, DateTimeOffset> eitherDateTime = EnsureNonDefault(markedAsFavoriteOn);

            Either<Error, (Owner favoredBy, DateTimeOffset markedAsFavoriteOn)> combined =
                from fb in eitherFavoredBy
                from dt in eitherDateTime
                select (fb, dt);

            return
                combined.Map(
                    combined =>
                        new FavoriteMark(
                            combined.favoredBy,
                            combined.markedAsFavoriteOn));
        }

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<FavoriteMark>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] FavoriteMark other)
            =>
                ValueObjectComparer<FavoriteMark>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<FavoriteMark>.Instance.GetHashCode();

        public static bool operator ==(FavoriteMark left, FavoriteMark right)
            =>
                ValueObjectComparer<FavoriteMark>.Instance.Equals(left, right);

        public static bool operator !=(FavoriteMark left, FavoriteMark right)
            =>
                !(left == right);
    }
}
