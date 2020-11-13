using System;

namespace Core.Application.Listings.Commands.AddLead
{
    public interface IAddLeadCommand
    {
        void Execute(Guid userId, AddLeadModel model);
    }
}