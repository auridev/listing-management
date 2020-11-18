using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class Phone : IEquatable<Phone>
    {
        private static readonly Regex _phoneValidationRegex = new Regex(@"^([+]?[\s0-9]+)?(\d{3}|[(]?[0-9]+[)])?([-]?[\s]?[0-9])+$", RegexOptions.Compiled);

        public string Number { get; }

        private Phone() { }

        private Phone(string number)
        {
            Number = number;
        }

        public static Phone Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException(nameof(number));

            string trimedNumber = number.Trim();

            if (trimedNumber.Length < 9)
                throw new ArgumentException(nameof(trimedNumber));

            if (!_phoneValidationRegex.IsMatch(trimedNumber))
                throw new ArgumentException(nameof(trimedNumber));

            return new Phone(trimedNumber);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Phone>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Phone other)
            => ValueObjectComparer<Phone>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Phone>.Instance.GetHashCode();

        public static bool operator ==(Phone left, Phone right)
            => ValueObjectComparer<Phone>.Instance.Equals(left, right);

        public static bool operator !=(Phone left, Phone right)
            => !(left == right);
    }
}
