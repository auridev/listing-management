using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.DeactivateSuspiciousListing
{
    public interface IDeactivateSuspiciousListingCommand
    {
        Either<Error, Unit> Execute(DeactivateSuspiciousListingModel model);
    }
}