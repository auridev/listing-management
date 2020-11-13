using Core.Domain.Common;
using Core.Domain.Messages;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.UnitTests.TestMocks
{
    internal class MessageMocks
    {
        public static Message Message_1 => new Message(
          Guid.NewGuid(),
          ValueObjectMocks.Recipient,
          ValueObjectMocks.Subject,
          ValueObjectMocks.MessageBody,
          Option<SeenDate>.None,
          DateTimeOffset.UtcNow);
    }
}
