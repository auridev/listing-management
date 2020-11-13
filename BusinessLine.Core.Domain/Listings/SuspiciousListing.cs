using Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace Core.Domain.Listings
{
    public sealed class SuspiciousListing : Listing
    {
        public DateTimeOffset MarkedAsSuspiciousAt { get; }
        public TrimmedString Reason { get; }

        private SuspiciousListing() { }

        public SuspiciousListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset markedAsSuspiciousAt,
            TrimmedString reason)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation)
        {
            if (markedAsSuspiciousAt == default)
                throw new ArgumentNullException(nameof(markedAsSuspiciousAt));
            if (reason == null)
                throw new ArgumentNullException(nameof(reason));

            MarkedAsSuspiciousAt = markedAsSuspiciousAt;
            Reason = reason;
        }

        public PassiveListing Deactivate(TrimmedString trimmedString, DateTimeOffset deactivationDate)
        {
            return new PassiveListing(Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                deactivationDate,
                trimmedString);
        }

        public ActiveListing Activate(DateTimeOffset expirationDate)
        {
            return new ActiveListing(
                Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                expirationDate);
        }
    }
}
