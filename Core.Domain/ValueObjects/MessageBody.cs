using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class MessageBody : IEquatable<MessageBody>
    {
        public string Content { get; }
        private MessageBody() { }
        private MessageBody(string content)
        {
            Content = content;
        }

        public static MessageBody Create(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException(nameof(content));

            return new MessageBody(content);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<MessageBody>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] MessageBody other)
            => ValueObjectComparer<MessageBody>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<MessageBody>.Instance.GetHashCode();

        public static bool operator ==(MessageBody left, MessageBody right)
            => ValueObjectComparer<MessageBody>.Instance.Equals(left, right);

        public static bool operator !=(MessageBody left, MessageBody right)
            => !(left == right);
    }
}
