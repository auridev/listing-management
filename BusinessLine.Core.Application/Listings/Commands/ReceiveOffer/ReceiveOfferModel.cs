using System;

namespace BusinessLine.Core.Application.Listings.Commands.ReceiveOffer
{
    public class ReceiveOfferModel
    {
        public Guid ListingId { get; set; }
        public decimal Value { get; set; }
        public string CurrencyCode { get; set; }
    }
}
