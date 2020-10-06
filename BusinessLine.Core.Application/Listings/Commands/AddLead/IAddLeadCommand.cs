using System;

namespace BusinessLine.Core.Application.Listings.Commands.AddLead
{
    public interface IAddLeadCommand
    {
        void Execute(Guid userId, AddLeadModel model);
    }
}