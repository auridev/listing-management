namespace Core.Application.Listings.Commands.ActivateSuspiciousListing
{
    public interface IActivateSuspiciousListingCommand
    {
        void Execute(ActivateSuspiciousListingModel model);
    }
}