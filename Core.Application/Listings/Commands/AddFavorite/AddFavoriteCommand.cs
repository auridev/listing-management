using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.AddFavorite
{
    public sealed class AddFavoriteCommand : IAddFavoriteCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _service;

        public AddFavoriteCommand(IListingRepository repository, IDateTimeService service)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _service = service ??
                throw new ArgumentNullException(nameof(service));
        }

        public Either<Error, Unit> Execute(Guid userId, AddFavoriteModel model)
        {
            Either<Error, Guid> eitherUserId = EnsureNonDefault(userId);
            Either<Error, AddFavoriteModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveListing> listing = FindActiveListing(eitherModel);
            Either<Error, FavoriteMark> favoriteMark =
                CreateFavoriteMark(
                    eitherUserId,
                    _service.GetCurrentUtcDateTime());

            Either<Error, Unit> markAsFavoriteResult =
                MarkAsFavorite(
                    listing,
                    favoriteMark);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    markAsFavoriteResult,
                    listing);

            return persistChangesResult;
        }

        private Either<Error, ActiveListing> FindActiveListing(Either<Error, AddFavoriteModel> eitherModel)
            =>
                eitherModel
                    .Map(model => _repository.FindActive(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active listing not found")));

        private Either<Error, FavoriteMark> CreateFavoriteMark(Either<Error, Guid> eitherUserId, DateTimeOffset markedAsFavoriteOn)
            =>
                eitherUserId
                    .Bind(userId => FavoriteMark.Create(userId, markedAsFavoriteOn));

        private Either<Error, Unit> MarkAsFavorite(Either<Error, ActiveListing> eitherActiveListing, Either<Error, FavoriteMark> eitherFavoriteMark)
            =>
                (
                    from favoriteMark in eitherFavoriteMark
                    from activeListing in eitherActiveListing
                    select (favoriteMark, activeListing)
                )
                .Bind(
                    context =>
                        context.activeListing.MarkAsFavorite(context.favoriteMark));

        private Either<Error, Unit> PersistChanges(Either<Error, Unit> eitherMarkAsFavorite, Either<Error, ActiveListing> eitherActiveListing)
            =>
                (
                    from markedSuccessfully in eitherMarkAsFavorite
                    from activeListing in eitherActiveListing
                    select (markedSuccessfully, activeListing)
                )
                .Map(context =>
                {
                    _repository.Update(context.activeListing);
                    _repository.Save();

                    return unit;
                });
    }
}
