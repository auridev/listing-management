using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

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

        public static Either<Error, MessageBody> Create(string content)
            =>
                EnsureNonEmpty(content)
                    .Bind(content => CreateMessageBody(content));

        private static Either<Error, MessageBody> CreateMessageBody(Either<Error, string> content)
            =>
                content.Map(content => new MessageBody(content));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<MessageBody>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] MessageBody other)
            =>
                ValueObjectComparer<MessageBody>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<MessageBody>.Instance.GetHashCode();

        public static bool operator ==(MessageBody left, MessageBody right)
            =>
                ValueObjectComparer<MessageBody>.Instance.Equals(left, right);

        public static bool operator !=(MessageBody left, MessageBody right)
            =>
                !(left == right);
    }
}
