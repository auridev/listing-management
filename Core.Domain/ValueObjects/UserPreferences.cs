using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class UserPreferences : IEquatable<UserPreferences>
    {
        public DistanceMeasurementUnit DistanceUnit { get; }
        public MassMeasurementUnit MassUnit { get; }
        public CurrencyCode CurrencyCode { get; }

        private UserPreferences() { }

        private UserPreferences(DistanceMeasurementUnit distanceUnit,
            MassMeasurementUnit massUnit,
            CurrencyCode currencyCode)
        {
            DistanceUnit = distanceUnit;
            MassUnit = massUnit;
            CurrencyCode = currencyCode;
        }

        public static UserPreferences Create(
            DistanceMeasurementUnit distanceUnit, 
            MassMeasurementUnit massUnit, 
            CurrencyCode currencyCode)
            => new UserPreferences(distanceUnit, massUnit, currencyCode);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<UserPreferences>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] UserPreferences other)
            => ValueObjectComparer<UserPreferences>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<UserPreferences>.Instance.GetHashCode();

        public static bool operator ==(UserPreferences left, UserPreferences right)
            => ValueObjectComparer<UserPreferences>.Instance.Equals(left, right);

        public static bool operator !=(UserPreferences left, UserPreferences right)
            => !(left == right);
    }
}
