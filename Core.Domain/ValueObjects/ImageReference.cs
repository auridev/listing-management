using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class ImageReference : IEquatable<ImageReference>
    {
        public Guid Id { get; }
        public Guid ParentReference { get; }
        public FileName FileName { get; }
        public FileSize FileSize { get; }

        private ImageReference() { }

        private ImageReference(Guid id, Guid parentReference, FileName fileName, FileSize fileSize)
        {
            Id = id;
            ParentReference = parentReference;
            FileName = fileName;
            FileSize = fileSize;
        }

        public static Either<Error, ImageReference> Create(Guid id, Guid parentReference, string fileName, long fileSize)
        {
            Either<Error, Guid> eitherId = EnsureNonDefault(id);
            Either<Error, Guid> eitherParentReference = EnsureNonDefault(parentReference);
            Either<Error, FileName> eitherFileName = FileName.Create(fileName);
            Either<Error, FileSize> eitherFileSize = FileSize.Create(fileSize);

            Either<Error, ImageReference> result =
            (
                from i in eitherId
                from pr in eitherParentReference
                from fn in eitherFileName
                from fs in eitherFileSize
                select (i, pr, fn, fs)
            ).Map(
                combined =>
                    new ImageReference(
                        combined.i,
                        combined.pr,
                        combined.fn,
                        combined.fs));

            return result;
        }


        public override bool Equals([AllowNull] object obj)
             =>
                ValueObjectComparer<ImageReference>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ImageReference other)
            =>
                ValueObjectComparer<ImageReference>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<ImageReference>.Instance.GetHashCode();

        public static bool operator ==(ImageReference left, ImageReference right)
            =>
                ValueObjectComparer<ImageReference>.Instance.Equals(left, right);

        public static bool operator !=(ImageReference left, ImageReference right)
            =>
                !(left == right);
    }
}
