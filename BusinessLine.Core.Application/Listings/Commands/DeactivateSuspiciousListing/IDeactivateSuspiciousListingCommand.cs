namespace Core.Application.Listings.Commands.DeactivateSuspiciousListing
{
    public interface IDeactivateSuspiciousListingCommand
    {
        void Execute(DeactivateSuspiciousListingModel model);
    }
}