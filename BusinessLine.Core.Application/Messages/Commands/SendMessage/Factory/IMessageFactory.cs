using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Messages;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Messages.Commands.SendMessage.Factory
{
    public interface IMessageFactory
    {
        Message Create(Recipient recipient, Subject subject, MessageBody messageBody);
    }
}