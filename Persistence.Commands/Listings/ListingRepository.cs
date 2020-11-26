using Core.Application.Listings.Commands;
using Core.Domain.Listings;
using LanguageExt;
using System;

namespace Persistence.Commands.Listings
{
    // all the Find methods use explicit casting to either Some or None
    // implicit cast wokrs as well but I want this to be more readable 
    public class ListingRepository : IListingRepository
    {
        private readonly CommandPersistenceContext _context;

        public ListingRepository(CommandPersistenceContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Add(NewListing newListing, ListingImageReference[] references)
        {
            if(newListing == null)
                throw new ArgumentNullException(nameof(newListing));
            if (references == null)
                throw new ArgumentNullException(nameof(references));


            _context.NewListings.Add(newListing);
            _context.ListingImageReferences.AddRange(references);
        }

        public void Add(ActiveListing activeListing)
        {
            if (activeListing == null)
                throw new ArgumentNullException(nameof(activeListing));

            _context.ActiveListings.Add(activeListing);
        }

        public void Add(PassiveListing passiveListing)
        {
            if (passiveListing == null)
                throw new ArgumentNullException(nameof(passiveListing));

            _context.PassiveListings.Add(passiveListing);
        }

        public void Add(SuspiciousListing suspiciousListing)
        {
            if (suspiciousListing == null)
                throw new ArgumentNullException(nameof(suspiciousListing));

            _context.SuspiciousListings.Add(suspiciousListing);
        }

        public void Add(ClosedListing closedListing)
        {
            if (closedListing == null)
                throw new ArgumentNullException(nameof(closedListing));

            _context.ClosedListings.Add(closedListing);
        }

        public void Update(ActiveListing activeListing)
        {
            
        }

        public void Delete(NewListing newListing)
        {
            if (newListing == null)
                throw new ArgumentNullException(nameof(newListing));

            _context.NewListings.Remove(newListing);
        }

        public void Delete(ActiveListing activeListing)
        {
            if (activeListing == null)
                throw new ArgumentNullException(nameof(activeListing));

            _context.ActiveListings.Remove(activeListing);
        }

        public void Delete(PassiveListing passiveListing)
        {
            if (passiveListing == null)
                throw new ArgumentNullException(nameof(passiveListing));

            _context.PassiveListings.Remove(passiveListing);
        }

        public void Delete(SuspiciousListing suspiciousListing)
        {
            if (suspiciousListing == null)
                throw new ArgumentNullException(nameof(suspiciousListing));

            _context.SuspiciousListings.Remove(suspiciousListing);
        }

        public Option<ActiveListing> FindActive(Guid id)
        {
            ActiveListing listing = _context.ActiveListings.Find(id);

            return listing != null 
                ? Option<ActiveListing>.Some(listing) 
                : Option<ActiveListing>.None;
        }

        public Option<NewListing> FindNew(Guid id)
        {
            NewListing listing = _context.NewListings.Find(id);

            return listing != null
                ? Option<NewListing>.Some(listing)
                : Option<NewListing>.None;
        }

        public Option<PassiveListing> FindPassive(Guid id)
        {
            PassiveListing listing = _context.PassiveListings.Find(id);

            return listing != null
                ? Option<PassiveListing>.Some(listing)
                : Option<PassiveListing>.None;
        }

        public Option<SuspiciousListing> FindSuspicious(Guid id)
        {
            SuspiciousListing listing = _context.SuspiciousListings.Find(id);

            return listing != null
                ? Option<SuspiciousListing>.Some(listing)
                : Option<SuspiciousListing>.None;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
