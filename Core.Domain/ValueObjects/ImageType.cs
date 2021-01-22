using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class ImageType : IEquatable<ImageType>
    {
        public static readonly ImageType Unknown = new ImageType(0, "unknown");
        public static readonly ImageType JPEG = new ImageType(10, "jpeg");
        public static readonly ImageType PNG = new ImageType(20, "png");
        public static readonly ImageType GIF = new ImageType(30, "gif");
        public static readonly ImageType BMP = new ImageType(40, "bmp");

        private static readonly Dictionary<string, ImageType> _types = new Dictionary<string, ImageType>
        {
            { JPEG.Value, JPEG },
            { PNG.Value, PNG },
            { GIF.Value, GIF },
            { BMP.Value, BMP }
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

            if (_types.ContainsKey(value))
                return _types[value];
            else
                return Unknown;
        }

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<ImageType>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ImageType other)
            =>
                ValueObjectComparer<ImageType>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<ImageType>.Instance.GetHashCode();

        public static bool operator ==(ImageType left, ImageType right)
            =>
                ValueObjectComparer<ImageType>.Instance.Equals(left, right);

        public static bool operator !=(ImageType left, ImageType right)
            =>
                !(left == right);
    }
}
