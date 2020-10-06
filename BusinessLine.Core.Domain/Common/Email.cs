using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class Email : IEquatable<Email>
    {
        private static readonly Regex _emailValidationRegex = new Regex(@"^(.+)@(.+)$", RegexOptions.Compiled);

        public string Value { get; }

        private Email() { }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            value = value.Trim();

            if (value.Length < 6)
                throw new ArgumentException(nameof(value));

            if (!_emailValidationRegex.IsMatch(value))
                throw new ArgumentException(nameof(value));

            return new Email(value);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Email>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Email other)
            => ValueObjectComparer<Email>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Email>.Instance.GetHashCode();

        public static bool operator ==(Email left, Email right)
            => ValueObjectComparer<Email>.Instance.Equals(left, right);

        public static bool operator !=(Email left, Email right)
            => !(left == right);
    }
}
