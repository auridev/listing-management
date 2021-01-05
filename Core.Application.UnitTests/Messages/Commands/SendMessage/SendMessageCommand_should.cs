using Core.Application.Messages.Commands;
using Core.Application.Messages.Commands.SendMessage;
using Core.Application.Messages.Commands.SendMessage.Factory;
using Core.Domain.ValueObjects;
using Core.Domain.Messages;
using Core.UnitTests.Mocks;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Messages.Commands.SendMessage
{
    public class SendMessageCommand_should
    {
        private readonly SendMessageCommand _sut;
        private readonly SendMessageModel _model;
        private readonly Message _message;
        private readonly AutoMocker _mocker;

        public SendMessageCommand_should()
        {
            _mocker = new AutoMocker();
            _message = FakesCollection.Message_1;
            _model = new SendMessageModel()
            {
                Recipient = Guid.NewGuid(),
                Subject = "aaaaa",
                Body = "bbbbb"
            };

            _mocker
                .GetMock<IMessageFactory>()
                .Setup(f => f.Create(It.IsAny<Recipient>(), It.IsAny<Subject>(), It.IsAny<MessageBody>()))
                .Returns(_message);

            _sut = _mocker.CreateInstance<SendMessageCommand>();
        }

        [Fact]
        public void create_message()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IMessageFactory>()
                .Verify(f => f.Create(It.IsAny<Recipient>(), It.IsAny<Subject>(), It.IsAny<MessageBody>()), Times.Once);
        }

        [Fact]
        public void add_message_to_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Add(_message), Times.Once);
        }

        [Fact]
        public void save_changes()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Save(), Times.Once);
        }
    }
}
