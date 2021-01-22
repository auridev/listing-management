using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
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

        public static Either<Error, GeographicLocation> Create(double latitude, double longitude)
        {
            Either<Error, double> eitherLatitude = EnsureLatitudeValid(latitude);
            Either<Error, double> eitherLongitude = EnsureLongitudeValid(longitude);

            Either<Error, (double latitude, double longitude)> combined =
                from lat in eitherLatitude
                from lon in eitherLongitude
                select (lat, lon);

            return
                combined.Map(
                    combined =>
                        new GeographicLocation(
                            combined.latitude,
                            combined.longitude));
        }

        private static Either<Error, double> EnsureLatitudeValid(double latitude)
            =>
                ((latitude < _minValidLatitude) || (latitude > _maxValidLatitude))
                    ? Left(Invalid<double>("latitude out of range"))
                    : Right(latitude);

        private static Either<Error, double> EnsureLongitudeValid(double longitude)
            =>
                ((longitude < _minValidLongitude) || (longitude > _maxValidLongitude))
                    ? Left(Invalid<double>("longitude out of range"))
                    : Right(longitude);

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<GeographicLocation>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] GeographicLocation other)
            =>
                ValueObjectComparer<GeographicLocation>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<GeographicLocation>.Instance.GetHashCode();

        public static bool operator ==(GeographicLocation left, GeographicLocation right)
            =>
                ValueObjectComparer<GeographicLocation>.Instance.Equals(left, right);

        public static bool operator !=(GeographicLocation left, GeographicLocation right)
            =>
                !(left == right);
    }
}
