using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries.GetPublicListings
{
    public interface IGetPublicListingsQuery
    {
        ICollection<PublicListingModel> Execute(Guid userId, GetPublicListingsQueryParams queryParams);
    }
}