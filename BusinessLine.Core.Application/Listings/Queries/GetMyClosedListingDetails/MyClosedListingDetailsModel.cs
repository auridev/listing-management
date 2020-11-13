﻿using Core.Application.Listings.Queries.Common;
using System;

namespace Core.Application.Listings.Queries.GetMyClosedListingDetails
{
    public class MyClosedListingDetailsModel : ListingDetailsModel
    {
        public DateTimeOffset ClosedOn { get; set; }
        public OfferDetailsModel AcceptedOffer { get; set; }
        public OfferOwnerContactDetailsModel OfferOwnerContactDetails { get; set; }
    }
}
