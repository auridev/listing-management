using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
{
    public sealed class FileSize : IEquatable<FileSize>
    {
        public long Bytes { get; }

        private FileSize() { }

        private FileSize(long bytes)
        {
            Bytes = bytes;
        }

        public static Either<Error, FileSize> Create(long bytes)
            =>
                EnsurePossitive(bytes)
                    .Map(bytes => new FileSize(bytes));

        private static Either<Error, long> EnsurePossitive(long bytes)
            =>
                (bytes > 0)
                    ? Right(bytes)
                    : Left(Invalid<long>("bytes need to be greater than 0"));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<FileSize>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] FileSize other)
            =>
                ValueObjectComparer<FileSize>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<FileSize>.Instance.GetHashCode();

        public static bool operator ==(FileSize left, FileSize right)
            =>
                ValueObjectComparer<FileSize>.Instance.Equals(left, right);

        public static bool operator !=(FileSize left, FileSize right)
            =>
                !(left == right);
    }
}
