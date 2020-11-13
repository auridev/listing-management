﻿using Core.Domain.Common;
using System;

namespace Core.Domain.Listings
{
    public sealed class ExpiredListing : Listing
    {
        public DateTimeOffset ExpiredOn { get; }
        public ExpiredListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset expiredOn)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation)
        {
            if (expiredOn == default)
                throw new ArgumentNullException(nameof(expiredOn));

            ExpiredOn = expiredOn;
        }
    }
}
