using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Listings
{
    public sealed class ClosedListing : Listing
    {
        public AcceptedOffer AcceptedOffer { get; }

        private readonly List<ClosedOffer> _closedOffers = new List<ClosedOffer>();
        public IReadOnlyList<ClosedOffer> ClosedOffers => _closedOffers.ToList();

        private ClosedListing()
        {
        }

        public ClosedListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate,
            AcceptedOffer acceptedOffer,
            List<ClosedOffer> closedOffers)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation, createdDate)
        {
            if (acceptedOffer == null)
                throw new ArgumentNullException(nameof(acceptedOffer));
            if (closedOffers == null)
                throw new ArgumentNullException(nameof(closedOffers));

            _closedOffers = closedOffers;
            AcceptedOffer = acceptedOffer;
        }
    }
}
