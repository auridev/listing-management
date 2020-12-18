using System;

namespace Core.Application.Listings.Queries.GetPublicListings
{
    public class PublicListingModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int MaterialTypeId { get; set; }
        public float Weight { get; set; }
        public string MassUnit { get; set; }
        public string City { get; set; }
        public bool HasMyOffer { get; set; }
    }
}
