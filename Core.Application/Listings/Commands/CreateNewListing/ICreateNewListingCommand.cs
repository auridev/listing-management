using System;
using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.CreateNewListing
{
    public interface ICreateNewListingCommand
    {
        Either<Error, Unit> Execute(Guid userId, CreateNewListingModel model);
    }
}