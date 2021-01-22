using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.MarkNewListingAsSuspicious
{
    public interface IMarkNewListingAsSuspiciousCommand
    {
        Either<Error, Unit> Execute(MarkNewListingAsSuspiciousModel model);
    }
}