using System;

namespace Core.Application.Listings.Queries.GetMyListings
{
    public class MyListingModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string MaterialType { get; set; }
    }
}
