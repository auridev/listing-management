using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands
{
    public interface IListingRepository
    {
        Option<NewListing> FindNew(Guid id);
        Option<SuspiciousListing> FindSuspicious(Guid id);
        Option<PassiveListing> FindPassive(Guid id);
        Option<ActiveListing> FindActive(Guid id);

        void Add(NewListing newListing, ListingImageReference[] references);
        void Add(ActiveListing activeListing);
        void Add(PassiveListing passiveListing);
        void Add(SuspiciousListing suspiciousListing);
        void Add(ClosedListing closedListing);

        void Update(ActiveListing activeListing);

        void Delete(NewListing newListing);
        void Delete(ActiveListing activeListing);
        void Delete(PassiveListing passiveListing);
        void Delete(SuspiciousListing suspiciousListing);

        void Save();
    }
}
