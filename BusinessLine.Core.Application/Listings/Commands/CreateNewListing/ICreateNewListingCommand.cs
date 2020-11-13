using System;

namespace Core.Application.Listings.Commands.CreateNewListing
{
    public interface ICreateNewListingCommand
    {
        void Execute(Guid userId, CreateNewListingModel model);
    }
}