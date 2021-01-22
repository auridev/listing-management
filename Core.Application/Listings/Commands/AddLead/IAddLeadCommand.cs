using System;
using Common.Helpers;
using LanguageExt;

namespace Core.Application.Listings.Commands.AddLead
{
    public interface IAddLeadCommand
    {
        Either<Error, Unit> Execute(Guid userId, AddLeadModel model);
    }
}