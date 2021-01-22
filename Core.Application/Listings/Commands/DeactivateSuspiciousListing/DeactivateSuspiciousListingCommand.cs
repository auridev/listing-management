using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.DeactivateSuspiciousListing
{
    public sealed class DeactivateSuspiciousListingCommand : IDeactivateSuspiciousListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public DeactivateSuspiciousListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, Unit> Execute(DeactivateSuspiciousListingModel model)
        {
            Either<Error, DeactivateSuspiciousListingModel> eitherModel = EnsureNotNull(model);
            Either<Error, SuspiciousListing> suspiciousListing = FindSuspiciousListing(eitherModel);
            Either<Error, TrimmedString> reason = CreateDeactivationReason(eitherModel);

            Either<Error, PassiveListing> passiveListing =
                Deactivate(
                    suspiciousListing,
                    reason,
                    _dateTimeService.GetCurrentUtcDateTime());
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    suspiciousListing,
                    passiveListing);

            return persistChangesResult;
        }

        private Either<Error, SuspiciousListing> FindSuspiciousListing(Either<Error, DeactivateSuspiciousListingModel> eitherModel)
           =>
                eitherModel
                    .Map(model => _repository.FindSuspicious(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("suspicious listing not found")));

        private Either<Error, TrimmedString> CreateDeactivationReason(Either<Error, DeactivateSuspiciousListingModel> eitherModel)
            =>
                eitherModel
                    .Bind(model => TrimmedString.Create(model.Reason));

        private Either<Error, PassiveListing> Deactivate(Either<Error, SuspiciousListing> eitherSuspiciousListing, Either<Error, TrimmedString> eitherReason, DateTimeOffset deactivationDate)
            =>
                (
                    from reason in eitherReason
                    from suspiciousListing in eitherSuspiciousListing
                    select (reason, suspiciousListing)
                )
                .Bind(
                    context =>
                        context.suspiciousListing.Deactivate(context.reason, deactivationDate));

        private Either<Error, Unit> PersistChanges(Either<Error, SuspiciousListing> eitherSuspiciousListing, Either<Error, PassiveListing> eitherPassiveListing)
            =>
                (
                    from suspiciousListing in eitherSuspiciousListing
                    from passiveListing in eitherPassiveListing
                    select (suspiciousListing, passiveListing)
                )
                .Map(context =>
                {
                    _repository.Delete(context.suspiciousListing);
                    _repository.Add(context.passiveListing);
                    _repository.Save();

                    return unit;
                });
    }
}
