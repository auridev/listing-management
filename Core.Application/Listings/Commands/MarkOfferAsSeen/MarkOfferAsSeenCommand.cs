using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.MarkOfferAsSeen
{
    public sealed class MarkOfferAsSeenCommand : IMarkOfferAsSeenCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly IDateTimeService _dateTimeService;

        public MarkOfferAsSeenCommand(IListingRepository listingRepository, IDateTimeService dateTimeService)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, Unit> Execute(MarkOfferAsSeenModel model)
        {
            Either<Error, MarkOfferAsSeenModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveListing> activeListing = FindActiveListing(eitherModel);
            Either<Error, SeenDate> seenDate = CreateSeenDate(_dateTimeService.GetCurrentUtcDateTime());

            Either<Error, Unit> markOfferAsSeenResult =
                MarkOfferAsSeen(activeListing, eitherModel, seenDate);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(markOfferAsSeenResult, activeListing);

            return persistChangesResult;
        }

        private Either<Error, ActiveListing> FindActiveListing(Either<Error, MarkOfferAsSeenModel> eitherModel)
           =>
                eitherModel
                    .Map(model => _listingRepository.FindActive(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active listing not found")));

        private Either<Error, SeenDate> CreateSeenDate(DateTimeOffset seenDate)
            =>
               SeenDate.Create(seenDate);

        private Either<Error, Unit> MarkOfferAsSeen(Either<Error, ActiveListing> eitherActiveListing, Either<Error, MarkOfferAsSeenModel> eitherModel, Either<Error, SeenDate> eitherSeenDate)
            =>
                (
                    from activeListing in eitherActiveListing
                    from model in eitherModel
                    from seenDate in eitherSeenDate
                    select (activeListing, model, seenDate)
                )
                .Bind(
                    context =>
                        context.activeListing.MarkOfferAsSeen(
                            context.model.OfferId, 
                            context.seenDate));

        private Either<Error, Unit> PersistChanges(Either<Error, Unit> eitherMarkOfferAsSeen, Either<Error, ActiveListing> eitherActiveListing)
            =>
                (
                    from markOfferAsSeen in eitherMarkOfferAsSeen
                    from activeListing in eitherActiveListing
                    select (markOfferAsSeen, activeListing)
                )
                .Map(context =>
                {
                    _listingRepository.Update(context.activeListing);
                    _listingRepository.Save();

                    return unit;
                });
    }
}
