using System;

namespace Core.Application.Listings.Commands.MarkNewListingAsSuspicious
{
    public class MarkNewListingAsSuspiciousModel
    {
        public Guid ListingId { get; set; }
        public string Reason { get; set; }
    }
}
