using System;
using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.ReceiveOffer
{
    public interface IReceiveOfferCommand
    {
        Either<Error, Unit> Execute(Guid userId, ReceiveOfferModel model);
    }
}