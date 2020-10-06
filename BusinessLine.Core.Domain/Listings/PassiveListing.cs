using BusinessLine.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Domain.Listings
{
    public sealed class PassiveListing : Listing
    {
        public DateTimeOffset DeactivationDate { get; }
        public TrimmedString Reason { get; }

        public PassiveListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset deactivationDate,
            TrimmedString reason) 
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation)
        {
            if (deactivationDate == default)
                throw new ArgumentNullException(nameof(deactivationDate));
            if (reason == null)
                throw new ArgumentNullException(nameof(reason));

            DeactivationDate = deactivationDate;
            Reason = reason;
        }

        public ActiveListing Reactivate(DateTimeOffset expirationDate)
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
