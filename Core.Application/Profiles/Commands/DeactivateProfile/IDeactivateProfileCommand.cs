using Common.Helpers;
using LanguageExt;

namespace Core.Application.Profiles.Commands.DeactivateProfile
{
    public interface IDeactivateProfileCommand
    {
        Either<Error, Unit> Execute(DeactivateProfileModel model);
    }
}