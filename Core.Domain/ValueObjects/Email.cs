using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        private static readonly int _minLength = 9;

        private static readonly Regex _emailValidationRegex = new Regex(@"^(.+)@(.+)$", RegexOptions.Compiled);

        public string Value { get; }

        private Email() { }

        private Email(string value)
        {
            Value = value;
        }

        public static Either<Error, Email> Create(string email)
            =>
                EnsureNonEmpty(email)
                    .Bind(email => Trim(email))
                    .Bind(email => EnsureMinLength(email, _minLength))
                    .Map(email => EnsureIsValidEmail(email))
                    .Bind(email => CreateEmail(email));

        private static Either<Error, Email> CreateEmail(Either<Error, string> email)
            =>
                email.Map(value => new Email(value));

        private static Either<Error, string> EnsureIsValidEmail(string email)
            =>
                _emailValidationRegex.IsMatch(email)
                    ? Right(email)
                    : Left(Invalid<string>("invalid email"));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Email>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Email other)
            =>
                ValueObjectComparer<Email>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Email>.Instance.GetHashCode();

        public static bool operator ==(Email left, Email right)
            =>
                ValueObjectComparer<Email>.Instance.Equals(left, right);

        public static bool operator !=(Email left, Email right)
            =>
                !(left == right);
    }
}
