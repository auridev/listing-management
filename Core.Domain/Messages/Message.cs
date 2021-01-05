using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Domain.Messages
{
    public sealed class Message : IEquatable<Message>
    {
        public Guid Id { get;  }
        public Recipient Recipient { get; }
        public Subject Subject { get; }
        public MessageBody Body { get; }
        public DateTimeOffset CreatedDate { get; }

        // due to ORM limitations
        public SeenDate ___efCoreSeenDate { get; private set; }
        public Option<SeenDate> SeenDate
        {
            get
            {
                return ___efCoreSeenDate == null ? Option<SeenDate>.None : ___efCoreSeenDate;
            }
            private set
            {
                value
                    .Some(v =>
                    {
                        ___efCoreSeenDate = v;
                    })
                    .None(() =>
                    {
                        ___efCoreSeenDate = null;
                    });
            }
        }


        private Message() { }

        public Message(Guid id, 
            Recipient recipient, 
            Subject subject, 
            MessageBody messageBody,
            Option<SeenDate> seenDate,
            DateTimeOffset createdDate)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));
            if (recipient == null)
                throw new ArgumentNullException(nameof(recipient));
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));
            if (messageBody == null)
                throw new ArgumentNullException(nameof(messageBody));
            if (createdDate == default)
                throw new ArgumentNullException(nameof(createdDate));

            Id = id;
            Recipient = recipient;
            Subject = subject;
            Body = messageBody;
            SeenDate = seenDate;
            CreatedDate = createdDate;
        }
        public void HasBeenSeen(SeenDate seenDate)
        {
            SeenDate = seenDate;
        }

        public bool Equals([AllowNull] Message other)
        {
            if (GetType() != other.GetType())
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id.IsDefault() || other.Id.IsDefault())
                return false;

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Message other))
                return false;

            return Equals(other);
        }

        public static bool operator ==(Message a, Message b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Message a, Message b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}
