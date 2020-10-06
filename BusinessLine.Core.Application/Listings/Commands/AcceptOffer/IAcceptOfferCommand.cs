namespace BusinessLine.Core.Application.Listings.Commands.AcceptOffer
{
    public interface IAcceptOfferCommand
    {
        void Execute(AcceptOfferModel model);
    }
}