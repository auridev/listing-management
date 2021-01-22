using Common.Helpers;
using LanguageExt;

namespace Core.Application.Profiles.Commands.MarkProfileAsIntroduced
{
    public interface IMarkProfileAsIntroducedCommand
    {
        Either<Error, Unit> Execute(MarkProfileAsIntroducedModel model);
    }
}