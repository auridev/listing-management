using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class FileSize : IEquatable<FileSize>
    {
        public long Bytes { get; }

        private FileSize() { }

        private FileSize(long bytes)
        {
            Bytes = bytes;
        }

        public static FileSize Create(long bytes)
        {
            if (bytes <= 0)
                throw new ArgumentException(nameof(bytes));

            return new FileSize(bytes);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<FileSize>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] FileSize other)
            => ValueObjectComparer<FileSize>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<FileSize>.Instance.GetHashCode();

        public static bool operator ==(FileSize left, FileSize right)
            => ValueObjectComparer<FileSize>.Instance.Equals(left, right);

        public static bool operator !=(FileSize left, FileSize right)
            => !(left == right);
    }
}
