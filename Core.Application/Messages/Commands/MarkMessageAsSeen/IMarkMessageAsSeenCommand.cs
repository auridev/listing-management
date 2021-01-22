using Common.Helpers;
using LanguageExt;

namespace Core.Application.Messages.Commands.MarkMessageAsSeen
{
    public interface IMarkMessageAsSeenCommand
    {
        Either<Error, Unit> Execute(MarkMessageAsSeenModel model);
    }
}