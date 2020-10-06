using System;

namespace BusinessLine.Core.Application.Listings.Commands.ReceiveOffer
{
    public interface IReceiveOfferCommand
    {
        void Execute(Guid userId, ReceiveOfferModel model);
    }
}