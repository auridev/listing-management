using System;
using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.RemoveFavorite
{
    public interface IRemoveFavoriteCommand
    {
        Either<Error, Unit> Execute(Guid userId, RemoveFavoriteModel model);
    }
}