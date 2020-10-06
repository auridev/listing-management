using BusinessLine.Core.Application.Listings.Queries.Common;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyNewListingDetails
{
    public class MyNewListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset CreatedOn { get; set; }
    }
}
