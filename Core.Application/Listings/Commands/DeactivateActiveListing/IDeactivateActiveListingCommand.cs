using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.DeactivateActiveListing
{
    public interface IDeactivateActiveListingCommand
    {
        Either<Error, Unit> Execute(DeactivateActiveListingModel model);
    }
}