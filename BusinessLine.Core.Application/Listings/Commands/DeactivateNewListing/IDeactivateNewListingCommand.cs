namespace BusinessLine.Core.Application.Listings.Commands.DeactivateNewListing
{
    public interface IDeactivateNewListingCommand
    {
        void Execute(DeactivateNewListingModel model);
    }
}