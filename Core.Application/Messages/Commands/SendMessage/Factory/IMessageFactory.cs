using Common.Helpers;
using Core.Domain.Messages;
using Core.Domain.ValueObjects;
using LanguageExt;

namespace Core.Application.Messages.Commands.SendMessage.Factory
{
    public interface IMessageFactory
    {
        Either<Error, Message> Create(Recipient recipient, Subject subject, MessageBody messageBody);
    }
}