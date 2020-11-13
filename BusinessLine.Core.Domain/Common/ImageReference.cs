using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class ImageReference : IEquatable<ImageReference>
    {
        public FileName FileName { get; }
        public FileSize FileSize { get; }

        private ImageReference() { }

        private ImageReference(FileName fileName, FileSize fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }

        public static ImageReference Create(FileName fileName, FileSize fileSize)
            => new ImageReference(fileName, fileSize);

        public override bool Equals([AllowNull] object obj)
             => ValueObjectComparer<ImageReference>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ImageReference other)
            => ValueObjectComparer<ImageReference>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<ImageReference>.Instance.GetHashCode();

        public static bool operator ==(ImageReference left, ImageReference right)
            => ValueObjectComparer<ImageReference>.Instance.Equals(left, right);

        public static bool operator !=(ImageReference left, ImageReference right)
            => !(left == right);
    }
}
