using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.ActivateSuspiciousListing
{
    public sealed class ActivateSuspiciousListingCommand : IActivateSuspiciousListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public ActivateSuspiciousListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, Unit> Execute(ActivateSuspiciousListingModel model)
        {
            Either<Error, ActivateSuspiciousListingModel> eitherModel = EnsureNotNull(model);
            Either<Error, SuspiciousListing> eitherSuspiciousListing = FindSuspiciousListing(eitherModel);

            Either<Error, ActiveListing> eitherActiveListing =
                Activate(
                    eitherSuspiciousListing,
                    _dateTimeService.GetFutureUtcDateTime(Listing.DaysUntilExpiration));
            Either<Error, Unit> result =
                PersistChanges(
                    eitherActiveListing,
                    eitherSuspiciousListing);

            return result;
        }

        private Either<Error, SuspiciousListing> FindSuspiciousListing(Either<Error, ActivateSuspiciousListingModel> eitherModel)
           =>
            eitherModel
                    .Map(model => _repository.FindSuspicious(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("suspicious listing not found")));

        private Either<Error, ActiveListing> Activate(Either<Error, SuspiciousListing> eitherSuspiciousListing, DateTimeOffset expirationDate)
            =>
                eitherSuspiciousListing
                    .Bind(listing =>
                            listing.Activate(expirationDate));

        private Either<Error, Unit> PersistChanges(
            Either<Error, ActiveListing> eitherActiveListing,
            Either<Error, SuspiciousListing> eitherSuspiciousListing)
            =>
                (
                    from activeListing in eitherActiveListing
                    from suspiciousListing in eitherSuspiciousListing
                    select (activeListing, suspiciousListing)
                )
                .Map(context =>
                {
                    _repository.Delete(context.suspiciousListing);
                    _repository.Add(context.activeListing);
                    _repository.Save();

                    return unit;
                });

    }
}
