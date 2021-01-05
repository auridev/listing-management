using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class ImageContent : IEquatable<ImageContent>
    {
        public FileName FileName { get; }
        public byte[] Content { get; }

        private ImageContent() { }

        private ImageContent(FileName fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
        }

        public static ImageContent Create(FileName fileName, byte[] content)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (content.Length == 0)
                throw new ArgumentNullException(nameof(content));

            return new ImageContent(fileName, content);
        }

        public override bool Equals([AllowNull] object obj)
             => ValueObjectComparer<ImageContent>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ImageContent other)
            => ValueObjectComparer<ImageContent>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<ImageContent>.Instance.GetHashCode();

        public static bool operator ==(ImageContent left, ImageContent right)
            => ValueObjectComparer<ImageContent>.Instance.Equals(left, right);

        public static bool operator !=(ImageContent left, ImageContent right)
            => !(left == right);
    }
}
