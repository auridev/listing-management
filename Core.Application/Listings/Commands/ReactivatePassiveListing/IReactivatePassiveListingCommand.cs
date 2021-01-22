using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.ReactivatePassiveListing
{
    public interface IReactivatePassiveListingCommand
    {
        Either<Error, Unit> Execute(ReactivatePassiveListingModel model);
    }
}