using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;

namespace BusinessLine.Core.Application.Listings.Commands.ReceiveOffer.Factory
{
    public interface IOfferFactory
    {
        Offer Create(Owner owner, MonetaryValue monetaryValue);
    }
}