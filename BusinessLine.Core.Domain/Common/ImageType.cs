using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class ImageType : IEquatable<ImageType>
    {
        public static readonly ImageType Unknown = new ImageType(0, "unknown");
        public static readonly ImageType JPEG = new ImageType(10, "jpeg");
        public static readonly ImageType PNG = new ImageType(20, "png");
        public static readonly ImageType GIF = new ImageType(30, "gif");
        public static readonly ImageType BMP = new ImageType(40, "bmp");

        private static readonly ImageType[] _allTypes = new ImageType[]
        {
            Unknown,
            JPEG,
            PNG,
            GIF,
            BMP
        };

        public string Value { get; }
        public int Id { get; }

        private ImageType() { }

        private ImageType(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static ImageType ByValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Unknown;

            var type = _allTypes.Find(t => t.Value.ToLower() == value.ToLower());

            return type
                .Some(t => t)
                .None(() => Unknown);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<ImageType>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ImageType other)
            => ValueObjectComparer<ImageType>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<ImageType>.Instance.GetHashCode();

        public static bool operator ==(ImageType left, ImageType right)
            => ValueObjectComparer<ImageType>.Instance.Equals(left, right);

        public static bool operator !=(ImageType left, ImageType right)
            => !(left == right);
    }
}
