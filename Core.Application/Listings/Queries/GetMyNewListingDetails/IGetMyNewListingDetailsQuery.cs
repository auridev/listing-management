using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyNewListingDetails
{
    public interface IGetMyNewListingDetailsQuery
    {
        Option<MyNewListingDetailsModel> Execute(Guid userId, Guid listingId);
    }
}