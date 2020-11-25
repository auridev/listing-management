﻿using Core.Domain.Common;
using Core.Domain.Offers;

namespace Core.Application.Listings.Commands.ReceiveOffer.Factory
{
    public interface IOfferFactory
    {
        ReceivedOffer Create(Owner owner, MonetaryValue monetaryValue);
    }
}