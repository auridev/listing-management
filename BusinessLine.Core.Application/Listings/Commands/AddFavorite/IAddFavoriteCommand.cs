using System;

namespace BusinessLine.Core.Application.Listings.Commands.AddFavorite
{
    public interface IAddFavoriteCommand
    {
        void Execute(Guid userId, AddFavoriteModel model);
    }
}