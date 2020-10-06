using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetPublicListings
{
    public class PublicListingModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string MaterialType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public float Weight { get; set; }
    }
}
