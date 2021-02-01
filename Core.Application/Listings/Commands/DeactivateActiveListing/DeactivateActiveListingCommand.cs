using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.DeactivateActiveListing
{
    public sealed class DeactivateActiveListingCommand : IDeactivateActiveListingCommand
    {
        private readonly IListingRepository _repository;

        public DeactivateActiveListingCommand(IListingRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Either<Error, Unit> Execute(DeactivateActiveListingModel model)
        {
            Either<Error, DeactivateActiveListingModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveListing> activeListing = FindActiveListing(eitherModel);
            Either<Error, TrimmedString> reason = CreateDeactivationReason(eitherModel);

            Either<Error, PassiveListing> passiveListing =
                Deactivate(
                    activeListing, 
                    reason);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    activeListing, 
                    passiveListing);

            return persistChangesResult;
        }

        private Either<Error, ActiveListing> FindActiveListing(Either<Error, DeactivateActiveListingModel> eitherModel)
            =>
                eitherModel
                    .Map(model => _repository.FindActive(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active listing not found")));

        private Either<Error, TrimmedString> CreateDeactivationReason(Either<Error, DeactivateActiveListingModel> eitherModel)
            =>
                eitherModel
                    .Bind(model => TrimmedString.Create(model.Reason));

        private Either<Error, PassiveListing> Deactivate(Either<Error, ActiveListing> eitherActiveListing, Either<Error, TrimmedString> eitherReason)
            =>
                (
                    from reason in eitherReason
                    from activeListing in eitherActiveListing
                    select (reason, activeListing)
                )
                .Bind(
                    context =>
                        context.activeListing.Deactivate(context.reason));

        private Either<Error, Unit> PersistChanges(Either<Error, ActiveListing> eitherActiveListing, Either<Error, PassiveListing> etherPassiveListing)
            =>
                (
                    from activeListing in eitherActiveListing
                    from passiveListing in etherPassiveListing
                    select (activeListing, passiveListing)
                )
                .Map(context =>
                {
                    _repository.Delete(context.activeListing);
                    _repository.Add(context.passiveListing);
                    _repository.Save();

                    return unit;
                });
    }
}
