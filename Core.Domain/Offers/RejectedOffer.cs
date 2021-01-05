using Core.Domain.ValueObjects;
using System;

namespace Core.Domain.Offers
{
    public class RejectedOffer : Offer
    {
        private RejectedOffer() { }
        public RejectedOffer(Guid id,
            Owner owner,
            MonetaryValue monetaryValue,
            DateTimeOffset createdDate)
            : base(id, owner, monetaryValue, createdDate)
        {
        }
    }
}
