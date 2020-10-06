using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Messages;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Messages.Commands.SendMessage.Factory
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

            return new Message(id, recipient, subject, messageBody, SeenDate.CreateNone(), createdDate);
        }
    }
}
