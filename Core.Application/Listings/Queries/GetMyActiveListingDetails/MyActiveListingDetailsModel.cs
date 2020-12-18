using Core.Application.Listings.Queries.Common;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries.GetMyActiveListingDetails
{
    public class MyActiveListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset ExpirationDate { get; set; }
        public List<OfferDetailsModel> ReceivedOffers { get; set; } = new List<OfferDetailsModel>();
    }
}
