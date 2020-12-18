using Core.Domain.Common;
using Core.Domain.Offers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Listings
{
    public sealed class ClosedListing : Listing
    {
        public DateTimeOffset ClosedOn { get; }
        public AcceptedOffer AcceptedOffer { get; }

        private readonly List<RejectedOffer> _rejectedOffers = new List<RejectedOffer>();
        public IReadOnlyList<RejectedOffer> RejectedOffers => _rejectedOffers.ToList();

        private ClosedListing() { }

        public ClosedListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate,
            DateTimeOffset closedOn,
            AcceptedOffer acceptedOffer,
            List<RejectedOffer> rejectedOffers)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation, createdDate)
        {
            if (closedOn == default)
                throw new ArgumentNullException(nameof(closedOn));
            if (acceptedOffer == null)
                throw new ArgumentNullException(nameof(acceptedOffer));
            if (rejectedOffers == null)
                throw new ArgumentNullException(nameof(rejectedOffers));

            ClosedOn = closedOn;
            AcceptedOffer = acceptedOffer;
            _rejectedOffers = rejectedOffers;
        }
    }
}
