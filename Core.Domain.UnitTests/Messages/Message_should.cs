using Common.Helpers;
using Core.Domain.Messages;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Messages
{
    public class Message_should
    {
        private readonly Message _sut;
        private static readonly Guid _id = Guid.NewGuid();
        private static readonly Recipient _recipient =
            TestValueObjectFactory.CreateRecipient(Guid.NewGuid());
        private static readonly Subject _subject =
            TestValueObjectFactory.CreateSubject("my subject");
        private static readonly MessageBody _messageBody =
            TestValueObjectFactory.CreateMessageBody("my first message");
        private static readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        public Message_should()
        {
            _sut = new Message(_id, _recipient, _subject, _messageBody, _createdDate);
        }

        [Fact]
        public void have_Id_property()
        {
            _sut.Id.Should().Be(_id);
        }

        [Fact]
        public void have_Recipient_property()
        {
            _sut.Recipient.Should().Be(_recipient);
        }

        [Fact]
        public void have_Subject_property()
        {
            _sut.Subject.Should().Be(_subject);
        }

        [Fact]
        public void have_Body_property()
        {
            _sut.Body.Should().Be(_messageBody);
        }

        [Fact]
        public void have_optional_SeenDate_property()
        {
            _sut.SeenDate.IsNone.Should().BeTrue();
        }

        [Fact]
        public void have_CreatedDate_property()
        {
            _sut.CreatedDate.Should().BeCloseTo(_createdDate);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { default, _recipient, _subject, _messageBody, _createdDate },
            new object[] { _id, null, _subject, _messageBody, _createdDate },
            new object[] { _id, _recipient, null, _messageBody, _createdDate },
            new object[] { _id, _recipient, _subject, null, _createdDate },
            new object[] { _id, _recipient, _subject, _messageBody, default }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void thrown_an_exception_during_creation_if_arguments_are_not_valid(
            Guid id,
            Recipient recipient,
            Subject subject,
            MessageBody messageBody,
            DateTimeOffset createdDate)
        {
            Action action = () => new Message(id, recipient, subject, messageBody, createdDate);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();

            var first = new Message(id, TestValueObjectFactory.CreateRecipient(Guid.NewGuid()), TestValueObjectFactory.CreateSubject("my subject 111"), _messageBody, _createdDate);
            var second = new Message(id, TestValueObjectFactory.CreateRecipient(Guid.NewGuid()), TestValueObjectFactory.CreateSubject("my subject 222"), _messageBody, _createdDate);

            // act
            var equals = first.Equals(second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = (object)new Message(id, TestValueObjectFactory.CreateRecipient(Guid.NewGuid()), TestValueObjectFactory.CreateSubject("my subject 111"), _messageBody, _createdDate);
            var second = (object)new Message(id, TestValueObjectFactory.CreateRecipient(Guid.NewGuid()), TestValueObjectFactory.CreateSubject("my subject 222"), _messageBody, _createdDate);

            // act
            var equals = first.Equals(second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new Message(id, TestValueObjectFactory.CreateRecipient(Guid.NewGuid()), TestValueObjectFactory.CreateSubject("my subject 111"), _messageBody, _createdDate);
            var second = new Message(id, TestValueObjectFactory.CreateRecipient(Guid.NewGuid()), TestValueObjectFactory.CreateSubject("my subject 222"), _messageBody, _createdDate);

            // act
            var equals = (first == second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_ids_dont_match()
        {
            // arrange
            var first = new Message(Guid.NewGuid(), _recipient, _subject, _messageBody, _createdDate);
            var second = new Message(Guid.NewGuid(), _recipient, _subject, _messageBody, _createdDate);

            // act
            var nonEquals = (first != second);

            // assert
            nonEquals.Should().BeTrue();
        }

        [Fact]
        public void have_the_same_hashcode_as_an_equal_listing()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new Message(id, TestValueObjectFactory.CreateRecipient(Guid.NewGuid()), TestValueObjectFactory.CreateSubject("my subject 222"), _messageBody, _createdDate);
            var second = new Message(id, TestValueObjectFactory.CreateRecipient(Guid.NewGuid()), TestValueObjectFactory.CreateSubject("my subject 333"), _messageBody, _createdDate);

            // act
            var equals = (first == second);
            var firstCode = first.GetHashCode();
            var secondCode = second.GetHashCode();

            // assert
            equals.Should().BeTrue();
            firstCode.Should().Be(secondCode);
        }

        [Fact]
        public void markable_as_seen_by_recipient()
        {
            // arrange 
            SeenDate seenDate = TestValueObjectFactory.CreateSeenDate(DateTimeOffset.UtcNow.AddDays(-3));
            Message message = new Message(_id, _recipient, _subject, _messageBody, _createdDate);

            // act
            Either<Error, Unit> action = message.HasBeenSeen(seenDate);

            // assert
            action
                .Right(_ =>
                {
                    message.SeenDate.IsSome.Should().BeTrue();
                    message.SeenDate.Some(seenDate => seenDate.Value.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(-3)));
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void ignore_invalid_seendatet()
        {
            // act
            Either<Error, Unit> action = _sut.HasBeenSeen(null);

            // assert
            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("seenDate");
                    error.Should().BeOfType<Error.Invalid>();
                });
        }
    }
}
