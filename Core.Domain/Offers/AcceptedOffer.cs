using Core.Domain.ValueObjects;
using System;

namespace Core.Domain.Offers
{
    public class AcceptedOffer : Offer
    {
        private AcceptedOffer() { }
        public AcceptedOffer(Guid id,
            Owner owner,
            MonetaryValue monetaryValue,
            DateTimeOffset createdDate)
            : base(id, owner, monetaryValue, createdDate)
        {
        }
    }
}
