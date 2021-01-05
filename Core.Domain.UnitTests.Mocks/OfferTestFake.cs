using Core.Domain.ValueObjects;
using Core.Domain.Offers;
using System;

namespace Core.UnitTests.Mocks
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
