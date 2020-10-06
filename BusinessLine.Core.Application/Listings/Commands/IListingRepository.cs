using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands
{
    public interface IListingRepository
    {
        Option<NewListing> FindNew(Guid id);

        Option<SuspiciousListing> FindSuspicious(Guid id);

        Option<PassiveListing> FindPassive(Guid id);

        Option<ActiveListing> FindActive(Guid id);

        Option<FavoriteUserListing> FindFavorite(Guid userId, Guid listingId);




        void Add(NewListing newListing, ListingImageReference[] references);

        void Add(ActiveListing activeListing);

        void Add(PassiveListing passiveListing);

        void Add(SuspiciousListing suspiciousListing);

        void Add(ClosedListing closedListing);

        void Add(FavoriteUserListing favoriteUserListing);

        void Add(Lead lead);



        void Delete(NewListing newListing);

        void Delete(ActiveListing activeListing);

        void Delete (PassiveListing passiveListing);

        void Delete(SuspiciousListing suspiciousListing);

        void Delete(FavoriteUserListing favoriteUserListing);





        void Save();
    }
}
