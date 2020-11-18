using Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace Core.Domain.Listings
{
    public sealed class NewListing : Listing
    {
        public DateTimeOffset CreatedDate { get; }

        private NewListing() { }

        public NewListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation)
        {
            if (createdDate == default)
                throw new ArgumentNullException(nameof(createdDate));

            CreatedDate = createdDate;
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

        public SuspiciousListing MarkAsSuspicious(DateTimeOffset markedAsSuspiciousAt, TrimmedString reason)
        {
            return new SuspiciousListing(
                Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                markedAsSuspiciousAt,
                reason);
        }
    }
}
