using System;

namespace BusinessLine.Core.Application.Listings.Commands.DeactivateSuspiciousListing
{
    public class DeactivateSuspiciousListingModel
    {
        public Guid ListingId { get; set; }
        public string Reason { get; set; }
    }
}
