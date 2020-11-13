using System;

namespace Core.Application.Listings.Commands.RemoveFavorite
{
    public interface IRemoveFavoriteCommand
    {
        void Execute(Guid userId, RemoveFavoriteModel model);
    }
}