using System;
using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.AddFavorite
{
    public interface IAddFavoriteCommand
    {
        Either<Error, Unit> Execute(Guid userId, AddFavoriteModel model);
    }
}