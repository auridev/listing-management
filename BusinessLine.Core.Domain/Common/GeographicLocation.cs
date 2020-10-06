using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class GeographicLocation : IEquatable<GeographicLocation>
    {
        private static readonly double _minValidLatitude = -85.05112878D;
        private static readonly double _maxValidLatitude = 85.05112878D;
        private static readonly double _minValidLongitude = -180D;
        private static readonly double _maxValidLongitude = 180D;

        public double Latitude { get; }

        public double Longitude { get; }

        private GeographicLocation() { }

        private GeographicLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static GeographicLocation Create(double latitude, double longitude)
        {
            if ((latitude < _minValidLatitude) || (latitude > _maxValidLatitude))
                throw new ArgumentException(nameof(latitude));
            if ((longitude < _minValidLongitude) || (longitude > _maxValidLongitude))
                throw new ArgumentException(nameof(longitude));

            return new GeographicLocation(latitude, longitude);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<GeographicLocation>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] GeographicLocation other)
            => ValueObjectComparer<GeographicLocation>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<GeographicLocation>.Instance.GetHashCode();

        public static bool operator ==(GeographicLocation left, GeographicLocation right)
            => ValueObjectComparer<GeographicLocation>.Instance.Equals(left, right);

        public static bool operator !=(GeographicLocation left, GeographicLocation right)
            => !(left == right);
    }
}
