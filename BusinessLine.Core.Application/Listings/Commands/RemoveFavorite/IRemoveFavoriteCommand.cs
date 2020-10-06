using System;

namespace BusinessLine.Core.Application.Listings.Commands.RemoveFavorite
{
    public interface IRemoveFavoriteCommand
    {
        void Execute(Guid userId, RemoveFavoriteModel model);
    }
}