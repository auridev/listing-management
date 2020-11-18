using System;

namespace Core.Application.Listings.Commands.DeactivateSuspiciousListing
{
    public class DeactivateSuspiciousListingModel
    {
        public Guid ListingId { get; set; }
        public string Reason { get; set; }
    }
}
