using System;

namespace BusinessLine.Core.Application.Profiles.Commands.UpdateProfile
{
    public interface IUpdateProfileCommand
    {
        void Execute(Guid profileId, UpdateProfileModel model);
    }
}