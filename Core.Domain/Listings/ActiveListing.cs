using Core.Domain.Common;
using Core.Domain.Offers;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Listings
{
    public sealed class ActiveListing : Listing
    {
        public DateTimeOffset ExpirationDate { get; }

        private readonly List<ReceivedOffer> _offers = new List<ReceivedOffer>();
        public IReadOnlyList<ReceivedOffer> Offers => _offers.ToList();

        private readonly List<Lead> _leads = new List<Lead>();
        public IReadOnlyList<Lead> Leads => _leads.ToList();

        private readonly List<FavoriteMark> _favorites = new List<FavoriteMark>();
        public IReadOnlyList<FavoriteMark> Favorites => _favorites.ToList();

        private ActiveListing() { }

        public ActiveListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset expirationDate)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation)
        {
            if (expirationDate == default)
                throw new ArgumentNullException(nameof(expirationDate));

            ExpirationDate = expirationDate;
        }

        public PassiveListing Deactivate(TrimmedString trimmedString, DateTimeOffset deactivationDate)
        {
            return new PassiveListing(Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                deactivationDate,
                trimmedString);
        }

        public void ReceiveOffer(ReceivedOffer offer)
        {
            if (offer == null)
                throw new ArgumentNullException(nameof(offer));

            if (Owner == offer.Owner) // ignore offers from owner of the listing 
                return;

            _offers // remove existing offers from the same owner
                .Find<ReceivedOffer>(o => o.Owner == offer.Owner)
                .IfSome(o => _offers.Remove(o));

            _offers.Add(offer);
        }

        public Option<ClosedListing> AcceptOffer(Guid receivedOfferId, DateTimeOffset closedOn)
        {
            // find the matching offer
            Option<ReceivedOffer> offer = 
                FindReceivedOfferById(receivedOfferId);
            if (offer.IsNone)
                return Option<ClosedListing>.None;

            // prerequisites
            AcceptedOffer acceptedOffer = null;
            List<RejectedOffer> rejectedOffers = new List<RejectedOffer>();

            // find accepted and rejected offers
            _offers.ForEach(o => 
            {
                if (o == offer)
                    acceptedOffer = new AcceptedOffer(o.Id, o.Owner, o.MonetaryValue, o.CreatedDate);
                else
                    rejectedOffers.Add(new RejectedOffer(o.Id, o.Owner, o.MonetaryValue, o.CreatedDate));
            });

            if (acceptedOffer != null)
                return new ClosedListing(Id, Owner, ListingDetails, ContactDetails, LocationDetails, GeographicLocation, closedOn, acceptedOffer, rejectedOffers);
            else
                return Option<ClosedListing>.None;
        }

        private Option<ReceivedOffer> FindReceivedOfferById(Guid receivedOfferId)
        {
            return _offers.Find<ReceivedOffer>(o => o.Id == receivedOfferId);
        }

        public void AddLead(Lead lead)
        {
            if (lead == null)
                throw new ArgumentNullException(nameof(lead));

            if (Owner == lead.UserInterested) // ignore leads from owner of the listing 
                return;

            _leads
                .Find<Lead>(l => l.UserInterested == lead.UserInterested)
                .IfNone(() => _leads.Add(lead));
        }

        public void MarkAsFavorite(FavoriteMark favorite)
        {
            if (favorite == null)
                throw new ArgumentNullException(nameof(favorite));

            if (Owner == favorite.FavoredBy) // ignore favorite marks from owner of the listing 
                return;

            _favorites
                .Find<FavoriteMark>(f => f.FavoredBy == favorite.FavoredBy)
                .IfNone(() => _favorites.Add(favorite));
        }

        public void RemoveFavorite(Owner favoredBy)
        {
            if (favoredBy == null)
                throw new ArgumentNullException(nameof(favoredBy));

            _favorites
               .Find<FavoriteMark>(f => f.FavoredBy == favoredBy)
               .IfSome(f => _favorites.Remove(f));
        }

        public void MarkOfferAsSeen(Guid offerId, SeenDate seenDate)
        {
            _offers
                .Find<ReceivedOffer>(o => o.Id == offerId)
                .IfSome(o => o.HasBeenSeen(seenDate));
        }
    }
}
