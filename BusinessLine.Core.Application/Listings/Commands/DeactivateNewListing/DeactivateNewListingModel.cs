using System;

namespace Core.Application.Listings.Commands.DeactivateNewListing
{
    public class DeactivateNewListingModel
    {
        public Guid ListingId { get; set; }
        public string Reason { get; set; }
    }
}
