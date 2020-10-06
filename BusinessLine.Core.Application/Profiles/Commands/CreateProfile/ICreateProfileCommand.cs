using System;

namespace BusinessLine.Core.Application.Profiles.Commands.CreateProfile
{
    public interface ICreateProfileCommand
    {
        void Execute(Guid userid, CreateProfileModel model);
    }
}