using Common.Helpers;
using Core.Application.Messages.Commands.SendMessage.Factory;
using Core.Domain.Messages;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Messages.Commands.SendMessage
{
    public sealed class SendMessageCommand : ISendMessageCommand
    {
        private readonly IMessageRepository _repository;
        private readonly IMessageFactory _factory;

        public SendMessageCommand(IMessageRepository repository, IMessageFactory factory)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _factory = factory ??
                throw new ArgumentNullException(nameof(factory));
        }

        public Either<Error, Unit> Execute(SendMessageModel model)
        {
            Either<Error, SendMessageModel> eitherModel = EnsureNotNull(model);
            Either<Error, Recipient> eitherRecipient = CreateRecipient(eitherModel);
            Either<Error, Subject> eitherSubject = CreateSubject(eitherModel);
            Either<Error, MessageBody> eitherMessageBody = CreateMessageBody(eitherModel);

            Either<Error, Unit> result =
                CreateMessage(
                    eitherRecipient,
                    eitherSubject,
                    eitherMessageBody)
                .Bind(
                    message => PersistChanges(message));

            return result;
        }


        private Either<Error, Recipient> CreateRecipient(Either<Error, SendMessageModel> eitherModel)
            =>
                eitherModel
                    .Bind(model => Recipient.Create(model.Recipient));

        private Either<Error, Subject> CreateSubject(Either<Error, SendMessageModel> eitherModel)
            =>
                eitherModel
                    .Bind(model => Subject.Create(model.Subject));

        private Either<Error, MessageBody> CreateMessageBody(Either<Error, SendMessageModel> eitherModel)
            =>
                eitherModel
                    .Bind(model => MessageBody.Create(model.Body));

        private Either<Error, Message> CreateMessage(
            Either<Error, Recipient> eitherRecipient,
            Either<Error, Subject> eitherSubject,
            Either<Error, MessageBody> eitherMessageBody)
            =>
                (
                    from recipient in eitherRecipient
                    from subject in eitherSubject
                    from messageBody in eitherMessageBody
                    select (recipient, subject, messageBody)
                )
                .Bind(
                    context =>
                        _factory.Create(
                            context.recipient,
                            context.subject,
                            context.messageBody));

        private Either<Error, Unit> PersistChanges(Either<Error, Message> eitherMessage)
            =>
                eitherMessage
                    .Map(message =>
                    {
                        _repository.Add(message);
                        _repository.Save();

                        return unit;
                    });
    }
}
