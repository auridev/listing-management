using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class FavoriteMark
    {
        public Owner FavoredBy { get; }
        public DateTimeOffset MarkedAsFavoriteOn { get; }

        private FavoriteMark() { }
        private FavoriteMark(Owner favoredBy, DateTimeOffset markedAsFavoriteOn)
        {
            if (favoredBy == null)
                throw new ArgumentNullException(nameof(favoredBy));
            if (markedAsFavoriteOn == default)
                throw new ArgumentNullException(nameof(markedAsFavoriteOn));

            FavoredBy = favoredBy;
            MarkedAsFavoriteOn = markedAsFavoriteOn;
        }

        public static FavoriteMark Create(Owner favoredBy, DateTimeOffset markedAsFavoriteOn)
            => new FavoriteMark(favoredBy, markedAsFavoriteOn);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<FavoriteMark>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] FavoriteMark other)
            => ValueObjectComparer<FavoriteMark>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<FavoriteMark>.Instance.GetHashCode();

        public static bool operator ==(FavoriteMark left, FavoriteMark right)
            => ValueObjectComparer<FavoriteMark>.Instance.Equals(left, right);

        public static bool operator !=(FavoriteMark left, FavoriteMark right)
            => !(left == right);
    }
}
