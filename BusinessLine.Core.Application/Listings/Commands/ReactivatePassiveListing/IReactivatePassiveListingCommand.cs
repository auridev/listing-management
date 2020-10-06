namespace BusinessLine.Core.Application.Listings.Commands.ReactivatePassiveListing
{
    public interface IReactivatePassiveListingCommand
    {
        void Execute(ReactivatePassiveListingModel model);
    }
}