using Common.Helpers;
using Core.Application.Messages.Commands;
using Core.Application.Messages.Commands.SendMessage;
using Core.Application.Messages.Commands.SendMessage.Factory;
using Core.Domain.Messages;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;
using static LanguageExt.Prelude;

namespace BusinessLine.Core.Application.UnitTests.Messages.Commands.SendMessage
{
    public class SendMessageCommand_should
    {
        private SendMessageCommand _sut;
        private SendMessageModel _model;
        private Message _message;

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public SendMessageCommand_should()
        {
            _mocker = new AutoMocker();
            _message = DummyData.Message_1;
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

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Add(_message), Times.Never);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Save(), Times.Never);
        }

        private void ExecuteWith_Success()
        {
            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherRight_on_success()
        {
            // act
            ExecuteWith_Success();

            // assert
            _executionResult
                .Right(u => u.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void persist_changes_on_success()
        {
            // act
            ExecuteWith_Success();

            // assert
            _mocker
                   .GetMock<IMessageRepository>()
                   .Verify(r => r.Add(_message), Times.Once);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        private void ExecuteWith_FailedRecipient()
        {
            _executionResult = _sut.Execute(new SendMessageModel()
            {
                Recipient = default,
                Subject = "aaaaa",
                Body = "bbbbb"
            });
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_recipient_creation_failed()
        {
            // act
            ExecuteWith_FailedRecipient();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("invalid guid");
                });
        }

        [Fact]
        public void not_persist_changes_when_recipient_creation_failed()
        {
            // act
            ExecuteWith_FailedRecipient();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedSubject()
        {
            _executionResult = _sut.Execute(new SendMessageModel()
            {
                Recipient = Guid.NewGuid(),
                Subject = string.Empty,
                Body = "bbbbb"
            });
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_subject_creation_failed()
        {
            // act
            ExecuteWith_FailedSubject();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("value cannot be empty");
                });
        }

        [Fact]
        public void not_persist_changes_when_subject_creation_failed()
        {
            // act
            ExecuteWith_FailedSubject();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedMessageBody()
        {
            _executionResult = _sut.Execute(new SendMessageModel()
            {
                Recipient = Guid.NewGuid(),
                Subject = "sadasdadsadsd",
                Body = string.Empty
            });
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_message_body_creation_failed()
        {
            // act
            ExecuteWith_FailedMessageBody();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("value cannot be empty");
                });
        }

        [Fact]
        public void not_persist_changes_when_message_body_creation_failed()
        {
            // act
            ExecuteWith_FailedMessageBody();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedMessage()
        {
            _mocker
                .GetMock<IMessageFactory>()
                .Setup(f => f.Create(It.IsAny<Recipient>(), It.IsAny<Subject>(), It.IsAny<MessageBody>()))
                .Returns(Left<Error, Message>(new Error.Invalid("some invalid message")));

            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_message_creation_failed()
        {
            // act
            ExecuteWith_FailedMessage();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("some invalid message");
                });
        }

        [Fact]
        public void not_persist_changes_when_message_creation_failed()
        {
            // act
            ExecuteWith_FailedMessageBody();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_NullModel()
        {
            _executionResult = _sut.Execute(null);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_model_is_null()
        {
            // act
            ExecuteWith_NullModel();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("cannot be null");
                });
        }

        [Fact]
        public void not_persist_changes_when_model_is_null()
        {
            // act
            ExecuteWith_NullModel();

            // assert
            VerifyChangesNotPersisted();
        }
    }
}
