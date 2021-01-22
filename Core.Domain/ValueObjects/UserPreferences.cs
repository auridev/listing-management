using Common.Helpers;
using LanguageExt;
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

        private UserPreferences(
            DistanceMeasurementUnit distanceUnit,
            MassMeasurementUnit massUnit,
            CurrencyCode currencyCode)
        {
            DistanceUnit = distanceUnit;
            MassUnit = massUnit;
            CurrencyCode = currencyCode;
        }

        public static Either<Error, UserPreferences> Create(
            string distanceUnit,
            string massUnit,
            string currencyCode)
        {
            Either<Error, DistanceMeasurementUnit> eitherDistanceUnit = DistanceMeasurementUnit.BySymbol(distanceUnit);
            Either<Error, MassMeasurementUnit> eitherMassUnit = MassMeasurementUnit.BySymbol(massUnit);
            Either<Error, CurrencyCode> eitherCurrencyCode = CurrencyCode.Create(currencyCode);

            Either<Error, (DistanceMeasurementUnit distanceUnit, MassMeasurementUnit massUnit, CurrencyCode currencyCode)> combined =
                from du in eitherDistanceUnit
                from mu in eitherMassUnit
                from cc in eitherCurrencyCode
                select (du, mu, cc);

            return
                combined.Map(
                    combined =>
                        new UserPreferences(
                            combined.distanceUnit,
                            combined.massUnit,
                            combined.currencyCode));
        }

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<UserPreferences>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] UserPreferences other)
            =>
                ValueObjectComparer<UserPreferences>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<UserPreferences>.Instance.GetHashCode();

        public static bool operator ==(UserPreferences left, UserPreferences right)
            =>
                ValueObjectComparer<UserPreferences>.Instance.Equals(left, right);

        public static bool operator !=(UserPreferences left, UserPreferences right)
            =>
                !(left == right);
    }
}
