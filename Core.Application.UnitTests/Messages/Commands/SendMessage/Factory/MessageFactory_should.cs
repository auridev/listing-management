using Core.Application.Messages.Commands.SendMessage.Factory;
using Core.Domain.ValueObjects;
using Core.Domain.Messages;
using Common.Dates;
using FluentAssertions;
using LanguageExt;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Messages.Commands.SendMessage.Factory
{
    public class MessageFactory_should
    {
        private readonly MessageFactory _sut;
        private readonly Recipient _recipient;
        private readonly Subject _subject;
        private readonly MessageBody _messageBody;
        private readonly Option<DateTimeOffset> _seenDate;
        private readonly AutoMocker _mocker;

        public MessageFactory_should()
        {
            _mocker = new AutoMocker();
            _recipient = Recipient.Create(Guid.NewGuid());
            _subject = Subject.Create("test subject");
            _messageBody = MessageBody.Create("beep");
            _seenDate = Option<DateTimeOffset>.None;
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<MessageFactory>();
        }


        [Fact]
        public void create_messages()
        {
            Message message = _sut.Create(_recipient, _subject, _messageBody);

            message.Should().NotBeNull();
            message.Id.Should().NotBeEmpty();
            message.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 5_000);
        }
    }
}
