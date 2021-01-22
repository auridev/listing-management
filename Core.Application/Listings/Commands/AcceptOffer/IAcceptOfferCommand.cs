using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.AcceptOffer
{
    public interface IAcceptOfferCommand
    {
        Either<Error, Unit> Execute(AcceptOfferModel model);
    }
}