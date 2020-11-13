using Core.Domain.Offers;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands
{
    public interface IOfferRepository
    {
        Option<ReceivedOffer> Find(Guid id);

        void Save();
    }
}
