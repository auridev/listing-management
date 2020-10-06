using System;

namespace BusinessLine.Core.Application.Listings.Commands.AcceptOffer
{
    public class AcceptOfferModel
    {
        public Guid ListingId { get; set; }
        public Guid OfferId { get; set; }
    }
}
