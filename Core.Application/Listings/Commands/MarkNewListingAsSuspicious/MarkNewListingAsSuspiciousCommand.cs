using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.MarkNewListingAsSuspicious
{
    public sealed class MarkNewListingAsSuspiciousCommand : IMarkNewListingAsSuspiciousCommand
    {
        private readonly IListingRepository _repository;

        public MarkNewListingAsSuspiciousCommand(IListingRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Either<Error, Unit> Execute(MarkNewListingAsSuspiciousModel model)
        {
            Either<Error, MarkNewListingAsSuspiciousModel> eitherModel = EnsureNotNull(model);
            Either<Error, NewListing> newListing = FindNewListing(eitherModel);
            Either<Error, TrimmedString> reason = CreateReason(eitherModel);

            Either<Error, SuspiciousListing> suspiciousListing =
                MarkAsSuspicious(
                    newListing,
                    reason);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    newListing,
                    suspiciousListing);

            return persistChangesResult;
        }

        private Either<Error, NewListing> FindNewListing(Either<Error, MarkNewListingAsSuspiciousModel> eitherModel)
           =>
                eitherModel
                    .Map(model => _repository.FindNew(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("new listing not found")));

        private Either<Error, TrimmedString> CreateReason(Either<Error, MarkNewListingAsSuspiciousModel> eitherModel)
            =>
                eitherModel
                    .Bind(model => TrimmedString.Create(model.Reason));

        private Either<Error, SuspiciousListing> MarkAsSuspicious(Either<Error, NewListing> eitherNewListing, Either<Error, TrimmedString> eitherReason)
            =>
                (
                    from newListing in eitherNewListing
                    from reason in eitherReason
                    select (newListing, reason)
                )
                .Bind(
                    context =>
                        context.newListing.MarkAsSuspicious(context.reason));

        private Either<Error, Unit> PersistChanges(Either<Error, NewListing> eitherNewListing, Either<Error, SuspiciousListing> eitherSuspiciousListing)
            =>
                (
                    from newListing in eitherNewListing
                    from suspiciousListing in eitherSuspiciousListing
                    select (newListing, suspiciousListing)
                )
                .Map(context =>
                {
                    _repository.Delete(context.newListing);
                    _repository.Add(context.suspiciousListing);
                    _repository.Save();

                    return unit;
                });
    }
}
