using System;

namespace BusinessLine.Core.Application.Listings.Commands.DeactivateNewListing
{
    public class DeactivateNewListingModel
    {
        public Guid ListingId { get; set; }
        public string Reason { get; set; }
    }
}
