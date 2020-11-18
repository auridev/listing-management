using Core.Domain.Common;
using Core.Domain.Messages;
using Common.Dates;
using LanguageExt;
using System;

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
        public Message Create(Recipient recipient, Subject subject, MessageBody messageBody)
        {
            var id = Guid.NewGuid();
            var createdDate = _dateTimeService.GetCurrentUtcDateTime();

            return new Message(id, recipient, subject, messageBody, Option<SeenDate>.None, createdDate);
        }
    }
}
