using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using LanguageExt;
using System;

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

        public void Execute(Guid userId, RemoveFavoriteModel model)
        {
            // Pre-requisites
            Owner favoredBy =
                Owner.Create(userId);
            Option<ActiveListing> optionalActiveListing =
                _repository.FindActive(model.ListingId);

            // Command
            optionalActiveListing
                .IfSome(listing =>
                {
                    listing.RemoveFavorite(favoredBy);

                    _repository.Update(listing);

                    _repository.Save();
                });
        }
    }
}
