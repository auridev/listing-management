using Core.Domain.Common;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using System;

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

        public void Execute(Guid userId, AddFavoriteModel model)
        {
            // Pre-requisites
            Owner favoredBy =
                Owner.Create(userId);
            FavoriteMark favoriteMark =
                FavoriteMark.Create(favoredBy, _service.GetCurrentUtcDateTime());
            Option<ActiveListing> optionalActiveListing =
                _repository.FindActive(model.ListingId);

            // Command
            optionalActiveListing
                .IfSome(listing =>
                {
                    listing.MarkAsFavorite(favoriteMark);

                    _repository.Save();
                });
        }
    }
}
