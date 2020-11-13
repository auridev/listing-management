using System;

namespace Core.Application.Listings.Commands.DeactivateActiveListing
{
    public class DeactivateActiveListingModel
    {
        public Guid ListingId { get; set; }
        public string Reason { get; set; }
    }
}
