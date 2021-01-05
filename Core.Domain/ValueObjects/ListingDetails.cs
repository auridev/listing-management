using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class ListingDetails : IEquatable<ListingDetails>
    {
        public Title Title { get; }
        public MaterialType MaterialType { get; }
        public Weight Weight { get; }
        public Description Description { get; }

        private ListingDetails() { }
        private ListingDetails(Title title, MaterialType materialType, Weight weight, Description description)
        {
            Title = title;
            MaterialType = materialType;
            Weight = weight;
            Description = description;
        }

        public static ListingDetails Create(Title title, MaterialType materialType, Weight weight, Description description)
            => new ListingDetails(title, materialType, weight, description);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<ListingDetails>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ListingDetails other)
            => ValueObjectComparer<ListingDetails>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<ListingDetails>.Instance.GetHashCode();

        public static bool operator ==(ListingDetails left, ListingDetails right)
            => ValueObjectComparer<ListingDetails>.Instance.Equals(left, right);

        public static bool operator !=(ListingDetails left, ListingDetails right)
            => !(left == right);

    }
}
