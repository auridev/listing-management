using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Messages;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Messages.Commands.MarkMessageAsSeen
{
    public sealed class MarkMessageAsSeenCommand : IMarkMessageAsSeenCommand
    {
        private readonly IMessageRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public MarkMessageAsSeenCommand(IMessageRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(MarkMessageAsSeenModel model)
        {
            DateTimeOffset dateTimeOffset = _dateTimeService.GetCurrentUtcDateTime();
            SeenDate seenDate = SeenDate.Create(dateTimeOffset);
            Option<Message> optionalMessage = _repository.Find(model.MessageId);

            optionalMessage
                .Some(m =>
                {
                    m.HasBeenSeen(seenDate);

                    _repository.Update(m);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
