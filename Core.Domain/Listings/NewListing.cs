using Core.Domain.ValueObjects;
using System;

namespace Core.Domain.Listings
{
    public sealed class NewListing : Listing
    {
        private NewListing() { }

        public NewListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation, createdDate)
        {
            if (createdDate == default)
                throw new ArgumentNullException(nameof(createdDate));
        }

        public PassiveListing Deactivate(TrimmedString trimmedString, DateTimeOffset deactivationDate)
        {
            return new PassiveListing(Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                CreatedDate,
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
                CreatedDate,
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
                CreatedDate,
                markedAsSuspiciousAt,
                reason);
        }
    }
}
