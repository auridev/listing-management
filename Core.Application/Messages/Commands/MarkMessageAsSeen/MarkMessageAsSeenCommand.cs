using Common.Dates;
using Common.Helpers;
using Core.Domain.Messages;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Messages.Commands.MarkMessageAsSeen
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

        public Either<Error, Unit> Execute(MarkMessageAsSeenModel model)
        {
            Either<Error, MarkMessageAsSeenModel> eitherModel = EnsureNotNull(model);
            Either<Error, Message> eitherMessage = FindMessage(eitherModel);
            Either<Error, SeenDate> eitherSeenDate = CreateSeenDate(eitherMessage, _dateTimeService.GetCurrentUtcDateTime());

            Either<Error, Unit> hasBeenSeenResult = HasBeenSeen(eitherMessage, eitherSeenDate);
            Either<Error, Unit> persistChangesResult = PersistChanges(hasBeenSeenResult, eitherMessage);

            return persistChangesResult;
        }

        private Either<Error, Message> FindMessage(Either<Error, MarkMessageAsSeenModel> eitherModel)
            =>
                 eitherModel
                    .Map(model => _repository.Find(model.MessageId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("message not found")));

        private Either<Error, SeenDate> CreateSeenDate(Either<Error, Message> eitherMessage, DateTimeOffset seenDate)
            =>
                eitherMessage
                    .Bind(_ => SeenDate.Create(seenDate));

        private Either<Error, Unit> HasBeenSeen(Either<Error, Message> eitherMessage, Either<Error, SeenDate> eitherSeenDate)
            =>
                (
                    from message in eitherMessage
                    from seenDate in eitherSeenDate
                    select (message, seenDate)
                )
                .Bind(
                    context =>
                        context.message.HasBeenSeen(context.seenDate));

        private Either<Error, Unit> PersistChanges(Either<Error, Unit> hasBeenSeenResult, Either<Error, Message> eitherMessage)
            =>
                (
                    from hasBeenSeen in hasBeenSeenResult
                    from message in eitherMessage
                    select (hasBeenSeen, message)
                )
                .Map(context =>
                {
                    _repository.Update(context.message);
                    _repository.Save();

                    return unit;
                });
    }
}
