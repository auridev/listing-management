using Core.Application.Listings.Queries.Common;
using System;

namespace Core.Application.Listings.Queries.GetMyExpiredListingDetails
{
    public class MyExpiredListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset ExpiredOn { get; set; }
    }
}
