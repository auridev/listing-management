using Core.Application.Listings.Queries.Common;
using System;

namespace Core.Application.Listings.Queries.GetMyActiveListingDetails
{
    public class MyActiveListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset ExpirationDate { get; set; }
        public OfferDetailsModel[] ReceivedOffers { get; set; }
    }
}
