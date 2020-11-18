using Core.Domain.Common;
using Core.Domain.Messages;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Messages
{
    public class Message_should
    {
        private readonly Message _sut;
        private static readonly Guid _id = Guid.NewGuid();
        private static readonly Recipient _recipient = Recipient.Create(Guid.NewGuid());
        private static readonly Subject _subject = Subject.Create("my subject");
        private static readonly MessageBody _messageBody = MessageBody.Create("my first message");
        private static readonly SeenDate _seenDate = SeenDate.Create(DateTimeOffset.UtcNow.AddDays(-1));
        private static readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;


        public Message_should()
        {
            _sut = new Message(_id, _recipient, _subject, _messageBody, _seenDate, _createdDate);
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
            _sut.SeenDate.Some(seenDate => seenDate.Value.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(-1), 5_000));
        }

        [Fact]
        public void have_CreatedDate_property()
        {
            _sut.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 5_000);
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
            Action action = () => new Message(id, recipient, subject, messageBody, _seenDate, createdDate);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new Message(id, Recipient.Create(Guid.NewGuid()), Subject.Create("my subject 111"), _messageBody, _seenDate, _createdDate);
            var second = new Message(id, Recipient.Create(Guid.NewGuid()), Subject.Create("my subject 222"), _messageBody, _seenDate, _createdDate);

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
            var first = (object)new Message(id, Recipient.Create(Guid.NewGuid()), Subject.Create("my subject 111"), _messageBody, _seenDate, _createdDate);
            var second = (object)new Message(id, Recipient.Create(Guid.NewGuid()), Subject.Create("my subject 222"), _messageBody, _seenDate, _createdDate);

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
            var first = new Message(id, Recipient.Create(Guid.NewGuid()), Subject.Create("my subject 111"), _messageBody, _seenDate, _createdDate);
            var second = new Message(id, Recipient.Create(Guid.NewGuid()), Subject.Create("my subject 222"), _messageBody, _seenDate, _createdDate);

            // act
            var equals = (first == second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_ids_dont_match()
        {
            // arrange
            var first = new Message(Guid.NewGuid(), _recipient, _subject, _messageBody, _seenDate, _createdDate);
            var second = new Message(Guid.NewGuid(), _recipient, _subject, _messageBody, _seenDate, _createdDate);

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
            var first = new Message(id, Recipient.Create(Guid.NewGuid()), Subject.Create("my subject 222"), _messageBody, _seenDate, _createdDate);
            var second = new Message(id, Recipient.Create(Guid.NewGuid()), Subject.Create("my subject 333"), _messageBody, _seenDate, _createdDate);

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
            var message = new Message(_id, _recipient, _subject, _messageBody, Option<SeenDate>.None, _createdDate);

            message.HasBeenSeen(SeenDate.Create(DateTimeOffset.UtcNow.AddDays(-3)));

            message.SeenDate.Some(seenDate => seenDate.Value.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(-3)));
        }
    }
}
