using System;
using Common.Helpers;
using LanguageExt;

namespace Core.Application.Profiles.Commands.CreateProfile
{
    public interface ICreateProfileCommand
    {
        Either<Error, Unit> Execute(Guid userid, CreateProfileModel model);
    }
}