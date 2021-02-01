using Common.Helpers;
using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using LanguageExt;

namespace Core.Application.Listings.Commands.ReceiveOffer.Factory
{
    public interface IOfferFactory
    {
        Either<Error, ActiveOffer> Create(Owner owner, MonetaryValue monetaryValue);
    }
}