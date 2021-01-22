using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.ActivateNewListing
{
    public interface IActivateNewListingCommand
    {
        Either<Error, Unit> Execute(ActivateNewListingModel model);
    }
}