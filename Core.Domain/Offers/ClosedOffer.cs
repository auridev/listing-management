using Core.Domain.ValueObjects;
using System;

namespace Core.Domain.Offers
{
    public class ClosedOffer : Offer
    {
        private ClosedOffer() { }
        public ClosedOffer(Guid id,
            Owner owner,
            MonetaryValue monetaryValue,
            DateTimeOffset createdDate)
            : base(id, owner, monetaryValue, createdDate)
        {
        }
    }
}
