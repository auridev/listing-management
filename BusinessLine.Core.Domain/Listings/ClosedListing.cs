using BusinessLine.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Domain.Listings
{
    public sealed class ClosedListing : Listing
    {
        public DateTimeOffset ClosedOn { get; }
        public Offer AcceptedOffer { get; }

        public ClosedListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset closedOn,
            Offer acceptedOffer)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation)
        {
            if (closedOn == default)
                throw new ArgumentNullException(nameof(closedOn));
            if (acceptedOffer == null)
                throw new ArgumentNullException(nameof(acceptedOffer));

            ClosedOn = closedOn;
            AcceptedOffer = acceptedOffer;
        }
    }
}
