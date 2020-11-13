using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries.GetMyListings
{
    public interface IGetMyListingsQuery
    {
        ICollection<MyListingModel> Execute(Guid userId, GetMyListingsQueryParams queryParams);
    }
}