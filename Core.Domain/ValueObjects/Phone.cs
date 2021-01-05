using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using U2U.ValueObjectComparers;
using static Common.Helpers.Result;
using static Common.Helpers.StringHelpers;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
{
    public sealed class Phone : IEquatable<Phone>
    {
        private static readonly int _minLength = 9;

        private static readonly Regex _phoneValidationRegex 
            = 
                new Regex(@"^([+]?[\s0-9]+)?(\d{3}|[(]?[0-9]+[)])?([-]?[\s]?[0-9])+$", RegexOptions.Compiled);

        public string Number { get; }

        private Phone() { }

        private Phone(string number)
        {
            Number = number;
        }

        public static Either<Error, Phone> Create(string number)
            =>
                EnsureNonEmpty(number)
                    .Bind(number => Trim(number))
                    .Bind(number => EnsureMinLength(number, _minLength))
                    .Map(number => EnsureIsValidNumber(number))
                    .Bind(number => CreatePhone(number));

        private static Either<Error, Phone> CreatePhone(Either<Error, string> number)
            =>
                number.Map(number => new Phone(number));

        private static Either<Error, string> EnsureIsValidNumber(string number)
            =>
                _phoneValidationRegex.IsMatch(number)
                    ? Right(number)
                    : Left(Invalid<string>("invalid phone number"));

        public override bool Equals([AllowNull] object obj)
            => 
                ValueObjectComparer<Phone>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Phone other)
            => 
                ValueObjectComparer<Phone>.Instance.Equals(this, other);

        public override int GetHashCode()
            => 
                ValueObjectComparer<Phone>.Instance.GetHashCode();

        public static bool operator ==(Phone left, Phone right)
            => 
                ValueObjectComparer<Phone>.Instance.Equals(left, right);

        public static bool operator !=(Phone left, Phone right)
            => 
                !(left == right);
    }
}
