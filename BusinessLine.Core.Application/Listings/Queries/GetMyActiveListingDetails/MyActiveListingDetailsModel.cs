using BusinessLine.Core.Application.Listings.Queries.Common;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyActiveListingDetails
{
    public class MyActiveListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset ExpirationDate { get; set; }
        public OfferDetailsModel[] ReceivedOffers { get; set; }
    }
}
