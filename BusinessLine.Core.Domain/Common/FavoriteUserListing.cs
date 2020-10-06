using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class FavoriteUserListing
    {
        public Guid Id { get; }
        public Owner Owner { get; }
        public Guid ListingId { get; }
        private FavoriteUserListing() { }
        private FavoriteUserListing(Guid id, Owner owner, Guid listingId)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (listingId == default)
                throw new ArgumentNullException(nameof(listingId));

            Id = id;
            Owner = owner;
            ListingId = listingId;
        }

        public static FavoriteUserListing Create(Guid id, Owner owner, Guid listingId)
            => new FavoriteUserListing(id, owner, listingId);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<FavoriteUserListing>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] FavoriteUserListing other)
            => ValueObjectComparer<FavoriteUserListing>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<FavoriteUserListing>.Instance.GetHashCode();

        public static bool operator ==(FavoriteUserListing left, FavoriteUserListing right)
            => ValueObjectComparer<FavoriteUserListing>.Instance.Equals(left, right);

        public static bool operator !=(FavoriteUserListing left, FavoriteUserListing right)
            => !(left == right);
    }
}
