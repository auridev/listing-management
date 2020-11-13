using Core.Domain.Common;
using Core.Domain.Offers;
using System;

namespace BusinessLine.Core.Domain.UnitTests.TestMocks
{
    // used only to test abstract offer stuff
    internal class OfferMock : Offer
    {
        public OfferMock(Guid id,
            Owner owner,
            MonetaryValue monetaryValue,
            DateTimeOffset createdDate)
            : base(id, owner, monetaryValue, createdDate)
        {

        }
    }
}
