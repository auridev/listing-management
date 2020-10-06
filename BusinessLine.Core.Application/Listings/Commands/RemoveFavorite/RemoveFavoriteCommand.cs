using BusinessLine.Core.Domain.Common;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.RemoveFavorite
{
    public sealed class RemoveFavoriteCommand : IRemoveFavoriteCommand
    {
        private readonly IListingRepository _repository;

        public RemoveFavoriteCommand(IListingRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public void Execute(Guid userId, RemoveFavoriteModel model)
        {
            // Pre-requisites
            Option<FavoriteUserListing> optionalFavoriteListing =
                _repository.FindFavorite(userId, model.ListingId);

            // Command
            optionalFavoriteListing
                .Some(listing =>
                {
                    _repository.Delete(listing);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
