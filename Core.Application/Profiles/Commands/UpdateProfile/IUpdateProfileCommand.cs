using System;
using Common.Helpers;
using LanguageExt;


namespace Core.Application.Profiles.Commands.UpdateProfile
{
    public interface IUpdateProfileCommand
    {
        Either<Error, Unit> Execute(Guid profileId, UpdateProfileModel model);
    }
}