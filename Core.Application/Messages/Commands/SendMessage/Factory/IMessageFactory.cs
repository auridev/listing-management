using Core.Domain.ValueObjects;
using Core.Domain.Messages;
using LanguageExt;
using System;

namespace Core.Application.Messages.Commands.SendMessage.Factory
{
    public interface IMessageFactory
    {
        Message Create(Recipient recipient, Subject subject, MessageBody messageBody);
    }
}