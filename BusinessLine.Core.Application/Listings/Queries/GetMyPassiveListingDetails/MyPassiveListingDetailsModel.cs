using BusinessLine.Core.Application.Listings.Queries.Common;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyPassiveListingDetails
{
    public class MyPassiveListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset DeactivationDate { get; set; }
        public string DeactivationReason { get; set; }
    }
}
