using Core.Application.Messages.Commands.SendMessage.Factory;
using Core.Domain.ValueObjects;
using Core.Domain.Messages;
using System;

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

        public void Execute(SendMessageModel model)
        {
            // Pre-requisites
            var recipient = Recipient.Create(model.Recipient);
            var subject = Subject.Create(model.Subject);
            var messageBody = MessageBody.Create(model.Body);

            // Command
            Message message = _factory.Create(recipient, subject, messageBody);

            _repository.Add(message);

            _repository.Save();
        }
    }
}
