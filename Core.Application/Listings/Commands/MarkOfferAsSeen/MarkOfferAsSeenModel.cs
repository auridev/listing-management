using System;

namespace Core.Application.Listings.Commands.MarkOfferAsSeen
{
    public class MarkOfferAsSeenModel
    {
        public Guid OfferId { get; set; }
        public Guid ListingId { get; set; }
    }
}
