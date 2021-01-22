using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.ReactivatePassiveListing
{
    public sealed class ReactivatePassiveListingCommand : IReactivatePassiveListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public ReactivatePassiveListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }
        public Either<Error, Unit> Execute(ReactivatePassiveListingModel model)
        {
            DateTimeOffset expirationDate = _dateTimeService.GetFutureUtcDateTime(Listing.DaysUntilExpiration);
            Either<Error, ReactivatePassiveListingModel> eitherModel = EnsureNotNull(model);
            Either<Error, PassiveListing> passiveListing = FindPassiveListing(eitherModel);

            Either<Error, ActiveListing> activeListing =
                Reactivate(passiveListing, expirationDate);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(passiveListing, activeListing);

            return persistChangesResult;
        }

        private Either<Error, PassiveListing> FindPassiveListing(Either<Error, ReactivatePassiveListingModel> eitherModel)
           =>
                eitherModel
                    .Map(model => _repository.FindPassive(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("passive listing not found")));

        private Either<Error, ActiveListing> Reactivate(Either<Error, PassiveListing> eitherPassiveListing, DateTimeOffset expirationDate)
            =>
                eitherPassiveListing
                    .Bind(passiveListing => passiveListing.Reactivate(expirationDate));


        private Either<Error, Unit> PersistChanges(Either<Error, PassiveListing> eitherPassiveListing, Either<Error, ActiveListing> eitherActiveListing)
            =>
                (
                    from passiveListing in eitherPassiveListing
                    from activeListing in eitherActiveListing
                    select (passiveListing, activeListing)
                )
                .Map(context =>
                {
                    _repository.Delete(context.passiveListing);
                    _repository.Add(context.activeListing);
                    _repository.Save();

                    return unit;
                });
    }
}
