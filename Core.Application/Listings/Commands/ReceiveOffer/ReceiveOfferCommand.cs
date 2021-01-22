using Common.Helpers;
using Core.Application.Listings.Commands.ReceiveOffer.Factory;
using Core.Domain.Listings;
using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.ReceiveOffer
{
    public sealed class ReceiveOfferCommand : IReceiveOfferCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly IOfferFactory _offerFactory;

        public ReceiveOfferCommand(IListingRepository listingRepository, IOfferFactory offerFactory)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _offerFactory = offerFactory ??
                throw new ArgumentNullException(nameof(offerFactory));
        }
        public Either<Error, Unit> Execute(Guid userId, ReceiveOfferModel model)
        {
            Either<Error, Guid> eitherUserId = EnsureNonDefault(userId);
            Either<Error, ReceiveOfferModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveListing> activeListing = FindActiveListing(eitherModel);
            Either<Error, Owner> owner = CreateOwner(eitherUserId);
            Either<Error, MonetaryValue> monetaryValue = CreateMonetaryValue(eitherModel);
            Either<Error, ReceivedOffer> receivedOffer = CreateOffer(owner, monetaryValue);

            Either<Error, Unit> receiveOfferResult =
                ReceiveOffer(activeListing, receivedOffer);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(receiveOfferResult, activeListing);

            return persistChangesResult;
        }

        private Either<Error, ActiveListing> FindActiveListing(Either<Error, ReceiveOfferModel> eitherModel)
           =>
            eitherModel
                    .Map(model => _listingRepository.FindActive(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active listing not found")));

        private Either<Error, Owner> CreateOwner(Either<Error, Guid> eitherUserId)
            =>
                eitherUserId
                    .Bind(userId => Owner.Create(userId));

        private Either<Error, MonetaryValue> CreateMonetaryValue(Either<Error, ReceiveOfferModel> eitherModel)
           =>
               eitherModel
                   .Bind(model => MonetaryValue.Create(model.Value, model.CurrencyCode));

        private Either<Error, ReceivedOffer> CreateOffer(Either<Error, Owner> eitherOwner, Either<Error, MonetaryValue> eitherMonetaryValue)
           =>
                (
                    from owner in eitherOwner
                    from monetaryValue in eitherMonetaryValue
                    select (owner, monetaryValue)
                )
                .Bind(
                    context =>
                        _offerFactory.Create(context.owner, context.monetaryValue));

        private Either<Error, Unit> ReceiveOffer(Either<Error, ActiveListing> eitherActiveListing, Either<Error, ReceivedOffer> eitherReceivedOffer)
            =>
                (
                    from activeListing in eitherActiveListing
                    from receivedOffer in eitherReceivedOffer
                    select (activeListing, receivedOffer)
                )
                .Bind(
                    context =>
                        context.activeListing.ReceiveOffer(context.receivedOffer));

        private Either<Error, Unit> PersistChanges(Either<Error, Unit> receiveOfferResult, Either<Error, ActiveListing> eitherActiveListing)
            =>
                (
                    from receiveOffer in receiveOfferResult
                    from activeListing in eitherActiveListing
                    select (receiveOffer, activeListing)
                )
                .Map(context =>
                {
                    _listingRepository.Update(context.activeListing);
                    _listingRepository.Save();

                    return unit;
                });
    }
}
