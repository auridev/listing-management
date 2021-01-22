using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using System;

namespace Test.Helpers
{
    public class OfferTestFake : Offer
    {
        public OfferTestFake(Guid id,
            Owner owner,
            MonetaryValue monetaryValue,
            DateTimeOffset createdDate)
            : base(id, owner, monetaryValue, createdDate)
        {
        }
    }
}
