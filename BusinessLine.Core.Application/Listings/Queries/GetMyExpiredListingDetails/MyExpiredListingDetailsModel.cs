using BusinessLine.Core.Application.Listings.Queries.Common;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyExpiredListingDetails
{
    public class MyExpiredListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset ExpiredOn { get; set; }
    }
}
