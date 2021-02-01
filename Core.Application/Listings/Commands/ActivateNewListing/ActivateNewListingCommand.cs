using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.ActivateNewListing
{
    public sealed class ActivateNewListingCommand : IActivateNewListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public ActivateNewListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, Unit> Execute(ActivateNewListingModel model)
        {
            Either<Error, NewListing> eitherNewListing =
                EnsureNotNull(model)
                    .Bind(eitherModel => FindNewListing(eitherModel));

            Either<Error, ActiveListing> eitherActiveListing =
                Activate(
                    eitherNewListing,
                    _dateTimeService.GetFutureUtcDateTime(Listing.DaysUntilExpiration));

            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    eitherNewListing,
                    eitherActiveListing);

            return persistChangesResult;
        }

        private Either<Error, NewListing> FindNewListing(Either<Error, ActivateNewListingModel> eitherModel)
            =>
                eitherModel
                    .Map(model => _repository.FindNew(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("new listing not found")));

        private Either<Error, ActiveListing> Activate(Either<Error, NewListing> eitherNewListing, DateTimeOffset expirationDate)
            =>
                eitherNewListing
                    .Bind(newListing =>
                        newListing.MarkAsActive(expirationDate));

        private Either<Error, Unit> PersistChanges(Either<Error, NewListing> eitherNewListing, Either<Error, ActiveListing> eitherActiveListing)
            =>
                (
                    from newListing in eitherNewListing
                    from activeListing in eitherActiveListing
                    select (newListing, activeListing)
                )
                .Map(context =>
                {
                    _repository.Delete(context.newListing);
                    _repository.Add(context.activeListing);
                    _repository.Save();

                    return unit;
                });
    }
}
