using Common.Helpers;
using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

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
            DateTimeOffset createdDate,
            DateTimeOffset expirationDate)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation, createdDate)
        {
            if (expirationDate == default)
                throw new ArgumentNullException(nameof(expirationDate));

            ExpirationDate = expirationDate;
        }

        public Either<Error, PassiveListing> Deactivate(TrimmedString reason, DateTimeOffset deactivationDate)
        {
            if (reason == null)
                return Invalid<PassiveListing>(nameof(reason));
            if (deactivationDate == default)
                return Invalid<PassiveListing>(nameof(deactivationDate));

            var passiveListing = new PassiveListing(
                Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                CreatedDate,
                deactivationDate,
                reason);

            return Success(passiveListing);
        }

        public Either<Error, Unit> ReceiveOffer(ReceivedOffer offer)
        {
            if (offer == null)
                return Invalid<Unit>("invalid offer");
            if (Owner == offer.Owner)
                return Invalid<Unit>("cannot accept offers from the listing owner");

            _offers // remove existing offers from the same owner
                .Find<ReceivedOffer>(o => o.Owner == offer.Owner)
                .IfSome(o => _offers.Remove(o));

            _offers.Add(offer);

            return Success(unit);
        }

        public Either<Error, ClosedListing> AcceptOffer(Guid receivedOfferId, DateTimeOffset closedOn)
        {
            if (receivedOfferId == default)
                return Invalid<ClosedListing>(nameof(receivedOfferId));
            if (closedOn == default)
                return Invalid<ClosedListing>(nameof(closedOn));


            // find the matching offer
            Option<ReceivedOffer> offer = FindReceivedOfferById(receivedOfferId);
            if (offer.IsNone)
                return NotFound<ClosedListing>("offer not found");

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
                return Success(new ClosedListing(Id, Owner, ListingDetails, ContactDetails, LocationDetails, GeographicLocation, CreatedDate, closedOn, acceptedOffer, rejectedOffers));
            else
                return NotFound<ClosedListing>("offer not found");
        }

        private Option<ReceivedOffer> FindReceivedOfferById(Guid receivedOfferId)
        {
            return _offers.Find<ReceivedOffer>(o => o.Id == receivedOfferId);
        }

        public Either<Error, Unit> AddLead(Lead lead)
        {
            if (lead == null)
                return Invalid<Unit>("invalid lead");
            if (Owner == lead.UserInterested)
                return Invalid<Unit>("cannot accept leads from the listing owner");

            Option<Lead> leadFromUser = _leads
                .Find<Lead>(l => l.UserInterested == lead.UserInterested);

            if (leadFromUser.IsSome)
                return Invalid<Unit>("lead from this user already exists");

            _leads.Add(lead);

            return Success(unit);
        }

        public Either<Error, Unit> MarkAsFavorite(FavoriteMark favorite)
        {
            if (favorite == null)
                return Invalid<Unit>("invalid favorite");
            if (Owner == favorite.FavoredBy)
                return Invalid<Unit>("cannot accept favorites from the listing owner");

            Option<FavoriteMark> existingFavoriteFromUser = _favorites
                .Find<FavoriteMark>(f => f.FavoredBy == favorite.FavoredBy);

            if (existingFavoriteFromUser.IsSome)
                return Invalid<Unit>("favorite from this user already exists");

            _favorites.Add(favorite);

            return Success(unit);
        }

        public Either<Error, Unit> RemoveFavorite(Owner favoredBy)
        {
            if (favoredBy == null)
                return Invalid<Unit>(nameof(favoredBy));

            Option<FavoriteMark> existingFavoriteFromUser = _favorites
                .Find<FavoriteMark>(f => f.FavoredBy == favoredBy);

            if (existingFavoriteFromUser.IsNone)
                return NotFound<Unit>(nameof(favoredBy));

            existingFavoriteFromUser
                .IfSome(f => _favorites.Remove(f));

            return Success(unit);
        }

        public Either<Error, Unit> MarkOfferAsSeen(Guid offerId, SeenDate seenDate)
        {
            if (offerId == default)
                return Invalid<Unit>(nameof(offerId));
            if (seenDate == null)
                return Invalid<Unit>(nameof(seenDate));

            Option<ReceivedOffer> offer = _offers
                .Find<ReceivedOffer>(o => o.Id == offerId);

            if (offer.IsNone)
                return NotFound<Unit>(nameof(offerId));

            offer.IfSome(o => o.HasBeenSeen(seenDate));

            return Success(unit);
        }
    }
}
