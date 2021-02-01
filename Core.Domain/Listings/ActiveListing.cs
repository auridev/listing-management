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
    public class ActiveListing : Listing
    {
        public DateTimeOffset ExpirationDate { get; }

        private readonly List<ActiveOffer> _activeOffers = new List<ActiveOffer>();
        public IReadOnlyList<ActiveOffer> ActiveOffers => _activeOffers.ToList();

        private readonly List<Lead> _leads = new List<Lead>();
        public IReadOnlyList<Lead> Leads => _leads.ToList();

        private readonly List<FavoriteMark> _favorites = new List<FavoriteMark>();
        public IReadOnlyList<FavoriteMark> Favorites => _favorites.ToList();

        private ActiveListing()
        {
        }

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

        public Either<Error, PassiveListing> Deactivate(TrimmedString reason)
        {
            if (reason == null)
                return Invalid<PassiveListing>(nameof(reason));

            var passiveListing = new PassiveListing(
                Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                CreatedDate,
                reason);

            return Success(passiveListing);
        }

        public Either<Error, Unit> ReceiveOffer(ActiveOffer offer)
        {
            if (offer == null)
                return Invalid<Unit>("invalid offer");
            if (Owner == offer.Owner)
                return Invalid<Unit>("cannot accept offers from the listing owner");

            _activeOffers // remove existing offers from the same owner
                .Find<ActiveOffer>(o => o.Owner == offer.Owner)
                .IfSome(o => _activeOffers.Remove(o));

            _activeOffers.Add(offer);

            return Success(unit);
        }

        public Either<Error, ClosedListing> AcceptOffer(Guid receivedOfferId, DateTimeOffset closedOn)
        {
            if (receivedOfferId == default)
                return Invalid<ClosedListing>(nameof(receivedOfferId));
            if (closedOn == default)
                return Invalid<ClosedListing>(nameof(closedOn));

            return
                FindReceivedOfferById(receivedOfferId)
                    .Map(
                        receivedOffer =>
                            new AcceptedOffer(
                                receivedOffer.Id,
                                receivedOffer.Owner,
                                receivedOffer.MonetaryValue,
                                receivedOffer.CreatedDate))
                    .Bind<ClosedListing>(
                        acceptedOffer =>
                            {
                                List<ClosedOffer> closedffers = _activeOffers
                                    .Map(o => new ClosedOffer(o.Id, o.Owner, o.MonetaryValue, o.CreatedDate))
                                    .ToList();

                                return new ClosedListing(
                                    Id,
                                    Owner,
                                    ListingDetails,
                                    ContactDetails,
                                    LocationDetails,
                                    GeographicLocation,
                                    CreatedDate,
                                    acceptedOffer,
                                    closedffers);
                            });
        }

        private Either<Error, ActiveOffer> FindReceivedOfferById(Guid receivedOfferId)
        {
            return
                _activeOffers
                    .Find<ActiveOffer>(o => o.Id == receivedOfferId)
                    .ToEither<Error>(new Error.NotFound("offer not found"));
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

            Option<ActiveOffer> offer = _activeOffers
                .Find<ActiveOffer>(o => o.Id == offerId);

            if (offer.IsNone)
                return NotFound<Unit>(nameof(offerId));

            offer.IfSome(o => o.HasBeenSeen(seenDate));

            return Success(unit);
        }
    }
}
