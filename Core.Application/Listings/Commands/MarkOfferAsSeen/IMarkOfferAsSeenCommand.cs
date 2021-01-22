using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.MarkOfferAsSeen
{
    public interface IMarkOfferAsSeenCommand
    {
        Either<Error, Unit> Execute(MarkOfferAsSeenModel model);
    }
}