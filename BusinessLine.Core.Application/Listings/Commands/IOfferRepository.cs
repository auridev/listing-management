using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands
{
    public interface IOfferRepository
    {
        Option<Offer> Find(Guid id);

        void Save();
    }
}
