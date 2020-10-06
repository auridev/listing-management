using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class MessageBody : IEquatable<MessageBody>
    {
        public Template Template { get; }

        [Ignore]
        public IEnumerable<TemplateParam> Params { get; }

        private MessageBody() { }
        private MessageBody(Template template, IEnumerable<TemplateParam> templateParams)
        {
            Template = template;
            Params = templateParams;
        }

        public static MessageBody Create(Template template, IEnumerable<TemplateParam> templateParams)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (templateParams == null)
                throw new ArgumentNullException(nameof(templateParams));

            return new MessageBody(template, templateParams);
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
