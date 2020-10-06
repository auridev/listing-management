using System;

namespace BusinessLine.Core.Application.Listings.Commands.CreateNewListing
{
    public interface ICreateNewListingCommand
    {
        void Execute(Guid userId, CreateNewListingModel model);
    }
}