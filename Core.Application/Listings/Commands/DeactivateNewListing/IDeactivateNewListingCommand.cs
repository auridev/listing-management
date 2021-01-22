using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.DeactivateNewListing
{
    public interface IDeactivateNewListingCommand
    {
        Either<Error, Unit> Execute(DeactivateNewListingModel model);
    }
}