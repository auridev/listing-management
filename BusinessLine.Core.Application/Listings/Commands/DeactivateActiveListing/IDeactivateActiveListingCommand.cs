namespace BusinessLine.Core.Application.Listings.Commands.DeactivateActiveListing
{
    public interface IDeactivateActiveListingCommand
    {
        void Execute(DeactivateActiveListingModel model);
    }
}