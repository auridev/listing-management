using Core.Application.Messages.Commands;
using Core.Application.Messages.Commands.MarkMessageAsSeen;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using Core.Domain.Messages;
using Common.Dates;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Messages.Commands.MarkMessageAsSeen
{
    public class MarkMessageAsSeenCommand_should
    {
        private readonly MarkMessageAsSeenCommand _sut;
        private readonly MarkMessageAsSeenModel _model;
        private readonly Message _message;
        private readonly AutoMocker _mocker;
        private readonly DateTimeOffset _seenDate = DateTimeOffset.UtcNow;
        private readonly Guid _messageId = Guid.NewGuid();

        public MarkMessageAsSeenCommand_should()
        {
            _mocker = new AutoMocker();
            _message = MessageMocks.Message_1;
            _model = new MarkMessageAsSeenModel()
            {
                MessageId = _messageId
            };

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(_seenDate);

            _mocker
                .GetMock<IMessageRepository>()
                .Setup(r => r.Find(_messageId))
                .Returns(Option<Message>.Some(_message));

            _sut = _mocker.CreateInstance<MarkMessageAsSeenCommand>();
        }

        [Fact]
        public void retrive_message_from_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Find(_messageId), Times.Once);
        }

        [Fact]
        public void udpate_message_in_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Update(It.IsAny<Message>()), Times.Once);
        }

        [Fact]
        public void save_changes()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void do_nothing_if_message_does_not_exist()
        {
            _mocker
                .GetMock<IMessageRepository>()
                .Setup(r => r.Find(_messageId))
                .Returns(Option<Message>.None);

            _sut.Execute(_model);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Update(It.IsAny<Message>()), Times.Never);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
