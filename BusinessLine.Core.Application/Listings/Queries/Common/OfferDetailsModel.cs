using System;

namespace BusinessLine.Core.Application.Listings.Queries.Common
{
    public class OfferDetailsModel
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public string CurrencyCode { get; set; }
    }
}
