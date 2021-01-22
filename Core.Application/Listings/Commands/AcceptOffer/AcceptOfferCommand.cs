using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.AcceptOffer
{
    public sealed class AcceptOfferCommand : IAcceptOfferCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly IDateTimeService _dateTimeService;

        public AcceptOfferCommand(IListingRepository listingRepository, IDateTimeService dateTimeService)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, Unit> Execute(AcceptOfferModel model)
        {
            Either<Error, AcceptOfferModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveListing> eitherActiveListing = FindActiveListing(eitherModel);

            Either<Error, ClosedListing> eitherClosedListing =
                AcceptOffer(
                    eitherActiveListing,
                    eitherModel,
                     _dateTimeService.GetCurrentUtcDateTime());

            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    eitherActiveListing,
                    eitherClosedListing);

            return persistChangesResult;
        }

        private Either<Error, ActiveListing> FindActiveListing(Either<Error, AcceptOfferModel> eitherModel)
            =>
                eitherModel
                    .Map(model => _listingRepository.FindActive(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active listing not found")));

        private Either<Error, ClosedListing> AcceptOffer(Either<Error, ActiveListing> eitherActiveListing, Either<Error, AcceptOfferModel> eitherModel, DateTimeOffset closedOn)
            =>
                (
                    from activeListing in eitherActiveListing
                    from model in eitherModel
                    select (activeListing, model)
                )
                .Bind(
                    context =>
                        context.activeListing.AcceptOffer(context.model.OfferId, closedOn));

        private Either<Error, Unit> PersistChanges(Either<Error, ActiveListing> eitherActiveListing, Either<Error, ClosedListing> eitherClosedListing)
            =>
                (
                    from activeListing in eitherActiveListing
                    from closedListing in eitherClosedListing
                    select (activeListing, closedListing)
                )
                .Map(context =>
                {
                    _listingRepository.Add(context.closedListing);
                    _listingRepository.Delete(context.activeListing);
                    _listingRepository.Save();

                    return unit;
                });
    }
}
