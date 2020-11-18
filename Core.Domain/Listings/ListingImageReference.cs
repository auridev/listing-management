using Core.Domain.Common;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Domain.Listings
{
    public class ListingImageReference : IEquatable<ListingImageReference>
    {
        public Guid Id { get; }
        public Guid ParentReference { get; }
        public FileName FileName { get; }
        public FileSize FileSize { get; }

        public ListingImageReference(Guid id, Guid parentReference, FileName fileName, FileSize fileSize)
        {
            Id = id;
            ParentReference = parentReference;
            FileName = fileName;
            FileSize = fileSize;
        }

        public bool Equals([AllowNull] ListingImageReference other)
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
            if (!(obj is Listing other))
                return false;

            return Equals(other);
        }

        public static bool operator ==(ListingImageReference a, ListingImageReference b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ListingImageReference a, ListingImageReference b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}
