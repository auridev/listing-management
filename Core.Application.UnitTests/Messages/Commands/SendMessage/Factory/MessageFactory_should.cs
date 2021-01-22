using Common.Dates;
using Common.Helpers;
using Core.Application.Messages.Commands.SendMessage.Factory;
using Core.Domain.Messages;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Messages.Commands.SendMessage.Factory
{
    public class MessageFactory_should
    {
        private readonly MessageFactory _sut;
        private readonly static Recipient _recipient = TestValueObjectFactory.CreateRecipient(Guid.NewGuid());
        private readonly static Subject _subject = TestValueObjectFactory.CreateSubject("test subject");
        private readonly static MessageBody _messageBody = TestValueObjectFactory.CreateMessageBody("beep");
        private readonly AutoMocker _mocker;

        public MessageFactory_should()
        {
            _mocker = new AutoMocker();
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<MessageFactory>();
        }

        [Fact]
        public void return_EitherRight_with_Message_on_success()
        {
            // act
            Either<Error, Message> eitherMessage = _sut.Create(_recipient, _subject, _messageBody);

            //assert
            eitherMessage
                .Right(offer => offer.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }


        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null, _subject, _messageBody, "recipient" },
            new object[] { _recipient, null, _messageBody, "subject" },
            new object[] { _recipient, _subject, null, "messageBody" }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void return_EitherLeft_with_proper_error_when_arguments_are_invalid(Recipient recipient, Subject subject, MessageBody messageBody, string errorMessage)
        {
            // act
            Either<Error, Message> eitherMessage = _sut.Create(recipient, subject, messageBody);

            //assert
            eitherMessage
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be(errorMessage);
                });
        }
    }
}
