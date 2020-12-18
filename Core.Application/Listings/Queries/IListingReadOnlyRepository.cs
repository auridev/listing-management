
using Core.Application.Helpers;
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
    public interface IListingReadOnlyRepository
    {
        Option<MyActiveListingDetailsModel> FindMyActive(Guid userId, Guid listingId);
        Option<MyClosedListingDetailsModel> FindMyClosed(Guid userId, Guid listingId);
        Option<MyExpiredListingDetailsModel> FindMyExpired(Guid userId, Guid listingId);
        Option<MyNewListingDetailsModel> FindMyNew(Guid userId, Guid listingId);
        Option<MyPassiveListingDetailsModel> FindMyPassive(Guid userId, Guid listingId);
        Option<PublicListingDetailsModel> FindPublic(Guid userId, Guid listingId);
        PagedList<MyListingModel> GetMy(Guid userId, GetMyListingsQueryParams queryParams);
        PagedList<PublicListingModel> GetPublic(Guid userId, GetPublicListingsQueryParams queryParams);
    }
}
