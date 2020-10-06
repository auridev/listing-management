namespace BusinessLine.Core.Application.Listings.Commands.ActivateNewListing
{
    public interface IActivateNewListingCommand
    {
        void Execute(ActivateNewListingModel model);
    }
}