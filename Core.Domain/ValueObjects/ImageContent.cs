using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
{
    public sealed class ImageContent : IEquatable<ImageContent>
    {
        // TODO 
        // Fix eqiality comparison for this

        public FileName FileName { get; }

        [Ignore]
        public byte[] Content { get; }

        private ImageContent() { }

        private ImageContent(FileName fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
        }

        public static Either<Error, ImageContent> Create(string fileName, byte[] content)
        {
            Either<Error, FileName> eitherFileName = FileName.Create(fileName);
            Either<Error, byte[]> eitherContent = EnsureValidContent(content);

            Either<Error, (FileName fileName, byte[] content)> combined =
                from fn in eitherFileName
                from c in eitherContent
                select (fn, c);

            return
                combined.Map(
                    combined =>
                        new ImageContent(combined.fileName, combined.content));
        }

        private static Either<Error, byte[]> EnsureValidContent(byte[] content)
            =>
                ((content != null) && (content.Length > 0))
                    ? Right(content)
                    : Left(Invalid<byte[]>("invalid content"));

        public override bool Equals([AllowNull] object obj)
             =>
                ValueObjectComparer<ImageContent>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ImageContent other)
            =>
                ValueObjectComparer<ImageContent>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<ImageContent>.Instance.GetHashCode();

        public static bool operator ==(ImageContent left, ImageContent right)
            =>
                ValueObjectComparer<ImageContent>.Instance.Equals(left, right);

        public static bool operator !=(ImageContent left, ImageContent right)
            =>
                !(left == right);
    }
}
