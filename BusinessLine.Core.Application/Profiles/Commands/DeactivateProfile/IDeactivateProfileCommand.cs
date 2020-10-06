namespace BusinessLine.Core.Application.Profiles.Commands.DeactivateProfile
{
    public interface IDeactivateProfileCommand
    {
        void Execute(DeactivateProfileModel model);
    }
}