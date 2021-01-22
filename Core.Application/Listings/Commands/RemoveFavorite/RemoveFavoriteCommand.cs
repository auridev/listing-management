using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static LanguageExt.Prelude;
using static Common.Helpers.Functions;

namespace Core.Application.Listings.Commands.RemoveFavorite
{
    public sealed class RemoveFavoriteCommand : IRemoveFavoriteCommand
    {
        private readonly IListingRepository _repository;

        public RemoveFavoriteCommand(IListingRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Either<Error, Unit> Execute(Guid userId, RemoveFavoriteModel model)
        {
            Either<Error, RemoveFavoriteModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveListing> activeListing = FindActiveListing(eitherModel);

            Either<Error, Owner> owner =
                CreateOwner(activeListing, userId);

            Either<Error, Unit> removeFavoriteResult =
                RemoveFavorite(activeListing, owner);

            Either<Error, Unit> persistChangesResult =
                PersistChanges(removeFavoriteResult, activeListing);

            return persistChangesResult;
        }

        private Either<Error, ActiveListing> FindActiveListing(Either<Error, RemoveFavoriteModel> eitherModel)
           =>
                eitherModel
                    .Map(model => _repository.FindActive(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active listing not found")));

        private Either<Error, Owner> CreateOwner(Either<Error, ActiveListing> eitherActiveListing, Guid userId)
            =>
                eitherActiveListing
                    .Bind(_ => Owner.Create(userId));

        private Either<Error, Unit> RemoveFavorite(Either<Error, ActiveListing> eitherActiveListing, Either<Error, Owner> eitherOwner)
            =>
                (
                    from activeListing in eitherActiveListing
                    from owner in eitherOwner
                    select (activeListing, owner)
                )
                .Bind(
                    context =>
                        context.activeListing.RemoveFavorite(context.owner));

        private Either<Error, Unit> PersistChanges(Either<Error, Unit> removeFavoriteResult, Either<Error, ActiveListing> eitherActiveListing)
            =>
                (
                    from removeFavorite in removeFavoriteResult
                    from activeListing in eitherActiveListing
                    select (removeFavorite, activeListing)
                )
                .Map(context =>
                {
                    _repository.Update(context.activeListing);
                    _repository.Save();

                    return unit;
                });
    }
}
