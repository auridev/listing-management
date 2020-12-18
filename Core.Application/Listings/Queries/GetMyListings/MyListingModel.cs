using Core.Application.Listings.Queries.Common;
using System;

namespace Core.Application.Listings.Queries.GetMyListings
{
    public class MyListingModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string MaterialTypeId { get; set; }
        public ListingType Type { get; set; }
    }
}
