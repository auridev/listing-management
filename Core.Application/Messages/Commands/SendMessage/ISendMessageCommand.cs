using Common.Helpers;
using LanguageExt;

namespace Core.Application.Messages.Commands.SendMessage
{
    public interface ISendMessageCommand
    {
        Either<Error, Unit> Execute(SendMessageModel model);
    }
}