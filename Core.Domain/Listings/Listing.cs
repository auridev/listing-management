﻿using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Domain.Listings
{
    public abstract class Listing : IEquatable<Listing>
    {
        public const int DaysUntilExpiration = 90;
        public Guid Id { get; }
        public Owner Owner { get; }
        public ListingDetails ListingDetails { get; }
        public ContactDetails ContactDetails { get; }
        public LocationDetails LocationDetails { get; }
        public GeographicLocation GeographicLocation { get; }
        public DateTimeOffset CreatedDate { get; }

        protected Listing()
        {
        }

        public Listing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (listingDetails == null)
                throw new ArgumentNullException(nameof(listingDetails));
            if (contactDetails == null)
                throw new ArgumentNullException(nameof(contactDetails));
            if (locationDetails == null)
                throw new ArgumentNullException(nameof(locationDetails));
            if (geographicLocation == null)
                throw new ArgumentNullException(nameof(geographicLocation));
            if (createdDate == default)
                throw new ArgumentNullException(nameof(createdDate));

            Id = id;
            Owner = owner;
            ListingDetails = listingDetails;
            ContactDetails = contactDetails;
            LocationDetails = locationDetails;
            GeographicLocation = geographicLocation;
            CreatedDate = createdDate;
        }

        public bool Equals([AllowNull] Listing other)
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

        public static bool operator ==(Listing a, Listing b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Listing a, Listing b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}