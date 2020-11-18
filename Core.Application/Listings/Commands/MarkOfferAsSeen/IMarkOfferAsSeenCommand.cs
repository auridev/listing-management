namespace Core.Application.Listings.Commands.MarkOfferAsSeen
{
    public interface IMarkOfferAsSeenCommand
    {
        void Execute(MarkOfferAsSeenModel model);
    }
}