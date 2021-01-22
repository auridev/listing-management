using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.ActivateSuspiciousListing
{
    public interface IActivateSuspiciousListingCommand
    {
        Either<Error, Unit> Execute(ActivateSuspiciousListingModel model);
    }
}