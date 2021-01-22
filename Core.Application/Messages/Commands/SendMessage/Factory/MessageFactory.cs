using Common.Dates;
using Common.Helpers;
using Core.Domain.Messages;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Result;

namespace Core.Application.Messages.Commands.SendMessage.Factory
{
    public sealed class MessageFactory : IMessageFactory
    {
        private readonly IDateTimeService _dateTimeService;

        public MessageFactory(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }
        public Either<Error, Message> Create(Recipient recipient, Subject subject, MessageBody messageBody)
        {
            if (recipient == null)
                return Invalid<Message>(nameof(recipient));
            if (subject == null)
                return Invalid<Message>(nameof(subject));
            if (messageBody == null)
                return Invalid<Message>(nameof(messageBody));

            var message = new Message(
                Guid.NewGuid(),
                recipient,
                subject,
                messageBody,
                _dateTimeService.GetCurrentUtcDateTime());

            return Success(message);
        }
    }
}
