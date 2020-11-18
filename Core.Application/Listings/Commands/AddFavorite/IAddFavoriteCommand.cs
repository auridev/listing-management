using System;

namespace Core.Application.Listings.Commands.AddFavorite
{
    public interface IAddFavoriteCommand
    {
        void Execute(Guid userId, AddFavoriteModel model);
    }
}