using BusinessLine.Core.Application.Helpers;

namespace BusinessLine.Core.Application.Listings.Queries.GetPublicListings
{
    public class GetPublicListingsQueryParams : ListQueryParams
    {
        public object MaterialTypeIds { get; set; }
        public string SearchParam { get; set; }
    }
}
