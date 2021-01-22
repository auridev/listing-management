using Common.Dates;
using Common.Helpers;
using Core.Application.Messages.Commands;
using Core.Application.Messages.Commands.MarkMessageAsSeen;
using Core.Domain.Messages;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Messages.Commands.MarkMessageAsSeen
{
    public class MarkMessageAsSeenCommand_should
    {
        private MarkMessageAsSeenCommand _sut;
        private MarkMessageAsSeenModel _model;
        private Message _message;
        private DateTimeOffset _seenDate = DateTimeOffset.UtcNow;
        private Guid _messageId = Guid.NewGuid();
        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public MarkMessageAsSeenCommand_should()
        {
            _mocker = new AutoMocker();
            _message = DummyData.Message_1;
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
                .Setup(r => r.Find(It.IsAny<Guid>()))
                .Returns(Option<Message>.Some(_message));

            _sut = _mocker.CreateInstance<MarkMessageAsSeenCommand>();
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Update(It.IsAny<Message>()), Times.Never);

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
                .Verify(r => r.Update(It.IsAny<Message>()), Times.Once);

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        private void ExecuteWith_FailedSeenDate()
        {
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns((DateTimeOffset)default);

            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_seen_date_creation_failed()
        {
            // act
            ExecuteWith_FailedSeenDate();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("invalid dateTimeOffset");
                });
        }

        [Fact]
        public void not_persist_changes_when_seen_date_creation_failed()
        {
            // act
            ExecuteWith_FailedSeenDate();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_MessageNotFound()
        {
            _mocker
                .GetMock<IMessageRepository>()
                .Setup(r => r.Find(It.IsAny<Guid>()))
                .Returns(Option<Message>.None);

            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_message_was_not_found()
        {
            // act
            ExecuteWith_MessageNotFound();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("message not found");
                });
        }

        [Fact]
        public void not_persist_changes_when_message_was_not_found()
        {
            // act
            ExecuteWith_MessageNotFound();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_InvalidModel()
        {
            _executionResult = _sut.Execute(null);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_model_is_null()
        {
            // act
            ExecuteWith_InvalidModel();

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
            ExecuteWith_InvalidModel();

            // assert
            VerifyChangesNotPersisted();
        }
    }
}
