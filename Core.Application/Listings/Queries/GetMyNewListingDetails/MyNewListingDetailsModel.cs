using Core.Application.Listings.Queries.Common;
using System;

namespace Core.Application.Listings.Queries.GetMyNewListingDetails
{
    public class MyNewListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset CreatedOn { get; set; }
    }
}
