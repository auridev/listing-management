using Core.Application.Listings.Queries.Common;
using System;

namespace Core.Application.Listings.Queries.GetMyClosedListingDetails
{
    public class MyClosedListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset ClosedOn { get; set; }
        public decimal AcceptedOfferValue { get; set; }
        public string AcceptedOfferCurrencyCode { get; set; }
        public OfferOwnerContactDetailsModel OfferOwnerContactDetails { get; set; }
    }
}
