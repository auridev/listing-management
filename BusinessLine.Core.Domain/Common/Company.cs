using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public class Company : IEquatable<Company>
    {
        public virtual TrimmedString Name { get; }

        protected Company() { }

        private Company(TrimmedString name)
        {
            Name = name;
        }

        public static Company Create(string name)
            => new Company((TrimmedString)name);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Company>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Company other)
            => ValueObjectComparer<Company>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Company>.Instance.GetHashCode();

        public static bool operator ==(Company left, Company right)
            => ValueObjectComparer<Company>.Instance.Equals(left, right);

        public static bool operator !=(Company left, Company right)
            => !(left == right);
    }
}
