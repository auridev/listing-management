
using Core.Application.Listings.Queries.GetMyActiveListingDetails;
using Core.Application.Listings.Queries.GetMyClosedListingDetails;
using Core.Application.Listings.Queries.GetMyExpiredListingDetails;
using Core.Application.Listings.Queries.GetMyListings;
using Core.Application.Listings.Queries.GetMyNewListingDetails;
using Core.Application.Listings.Queries.GetMyPassiveListingDetails;
using Core.Application.Listings.Queries.GetPublicListingDetails;
using Core.Application.Listings.Queries.GetPublicListings;
using LanguageExt;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries
{
    public interface IListingDataService
    {
        Option<MyActiveListingDetailsModel> FindMyActive(Guid userId, GetMyActiveListingDetailsQueryParams queryParams);
        Option<MyClosedListingDetailsModel> FindMyClosed(Guid userId, GetMyClosedListingDetailsQueryParams queryParams);
        Option<MyExpiredListingDetailsModel> FindMyExpired(Guid userId, GetMyExpiredListingDetailsQueryParams queryParams);
        Option<MyNewListingDetailsModel> FindMyNew(Guid userId, GetMyNewListingDetailsQueryParams queryParams);
        Option<MyPassiveListingDetailsModel> FindMyPassive(Guid userId, GetMyPassiveListingDetailsQueryParams queryParams);
        Option<PublicListingDetailsModel> FindPublic(Guid userId, GetPublicListingDetailsQueryParams queryParams);
        ICollection<MyListingModel> GetMy(Guid userId, GetMyListingsQueryParams queryParams);
        ICollection<PublicListingModel> GetPublic(Guid userId, GetPublicListingsQueryParams queryParams);
    }
}
