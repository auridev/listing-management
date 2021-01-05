using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class TemplateParam : IEquatable<TemplateParam>
    {
        public string Tag { get; }
        public string Value { get; }

        private TemplateParam() { }
        private TemplateParam(string tag, string value)
        {
            Tag = tag;
            Value = value;
        }

        public static TemplateParam Create(string tag, string value)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentNullException(nameof(tag));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            return new TemplateParam(tag, value);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<TemplateParam>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] TemplateParam other)
            => ValueObjectComparer<TemplateParam>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<TemplateParam>.Instance.GetHashCode();

        public static bool operator ==(TemplateParam left, TemplateParam right)
            => ValueObjectComparer<TemplateParam>.Instance.Equals(left, right);

        public static bool operator !=(TemplateParam left, TemplateParam right)
            => !(left == right);

    }
}
