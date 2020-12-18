using Core.Application.Helpers;

namespace Core.Application.Listings.Queries.GetPublicListings
{
    public class GetPublicListingsQueryParams : ListQueryParams
    {
        public object MaterialTypeIds { get; set; }
        public string SearchParam { get; set; }
        public bool OnlyWithMyOffers { get; set; }
    }
}
