using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.AddFavorite
{
    public sealed class AddFavoriteCommand : IAddFavoriteCommand
    {
        private readonly IListingRepository _repository;

        public AddFavoriteCommand(IListingRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public void Execute(Guid userId, AddFavoriteModel model)
        {
            // Pre-requisites
            Owner owner =
                Owner.Create(userId);
            Option<ActiveListing> optionalActiveListing =
                _repository.FindActive(model.ListingId);

            // Command
            optionalActiveListing
                .Some(listing =>
                {
                    if (listing.Owner == owner)
                        return;

                    Guid id = Guid.NewGuid();
                    var favoriteUserListing = FavoriteUserListing.Create(id,
                        owner,
                        model.ListingId);

                    _repository.Add(favoriteUserListing);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
