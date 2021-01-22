using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;


namespace Core.Application.Listings.Commands.DeactivateNewListing
{
    public sealed class DeactivateNewListingCommand : IDeactivateNewListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public DeactivateNewListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, Unit> Execute(DeactivateNewListingModel model)
        {
            Either<Error, DeactivateNewListingModel> eitherModel = EnsureNotNull(model);
            Either<Error, NewListing> newListing = FindNewListing(eitherModel);
            Either<Error, TrimmedString> reason = CreateDeactivationReason(eitherModel);

            Either<Error, PassiveListing> passiveListing =
                Deactivate(
                    newListing,
                    reason,
                    _dateTimeService.GetCurrentUtcDateTime());
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    newListing,
                    passiveListing);

            return persistChangesResult;
        }

        private Either<Error, NewListing> FindNewListing(Either<Error, DeactivateNewListingModel> eitherModel)
           =>
                eitherModel
                    .Map(model => _repository.FindNew(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("new listing not found")));

        private Either<Error, TrimmedString> CreateDeactivationReason(Either<Error, DeactivateNewListingModel> eitherModel)
            =>
                eitherModel
                    .Bind(model => TrimmedString.Create(model.Reason));

        private Either<Error, PassiveListing> Deactivate(Either<Error, NewListing> eitherNewListing, Either<Error, TrimmedString> eitherReason, DateTimeOffset deactivationDate)
            =>
                (
                    from reason in eitherReason
                    from newListing in eitherNewListing
                    select (reason, newListing)
                )
                .Bind(
                    context =>
                        context.newListing.Deactivate(
                            context.reason,
                            deactivationDate));

        private Either<Error, Unit> PersistChanges(Either<Error, NewListing> eitherNewListing, Either<Error, PassiveListing> eitherPassiveListing)
            =>
                (
                    from newListing in eitherNewListing
                    from passiveListing in eitherPassiveListing
                    select (newListing, passiveListing)
                )
                .Map(context =>
                {
                    _repository.Delete(context.newListing);
                    _repository.Add(context.passiveListing);
                    _repository.Save();

                    return unit;
                });
    }
}
