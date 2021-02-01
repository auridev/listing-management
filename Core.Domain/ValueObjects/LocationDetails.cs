using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static LanguageExt.Prelude;

namespace Core.Domain.ValueObjects
{
    public sealed class LocationDetails : IEquatable<LocationDetails>
    {
        public Alpha2Code CountryCode { get; }
        public City City { get; }
        public PostCode PostCode { get; }
        public Address Address { get; }

        //// this is to overcome current ORM limitations
        public State ___efCoreState { get; private set; }

        public Option<State> State
        {
            get
            {
                return ___efCoreState == null ? Option<State>.None : ___efCoreState;
            }
            private set
            {
                value
                    .Some(v =>
                    {
                        ___efCoreState = v;
                    })
                    .None(() =>
                    {
                        ___efCoreState = null;
                    });
            }
        }

        private LocationDetails() { }

        private LocationDetails(
            Alpha2Code countryCode,
            Option<State> state,
            City city,
            PostCode postCode,
            Address address)
        {
            CountryCode = countryCode;
            State = state;
            City = city;
            PostCode = postCode;
            Address = address;
        }

        public static Either<Error, LocationDetails> Create(
            string alpha2CountryCode,
            string state,
            string city,
            string postCode,
            string address)
        {
            Either<Error, Alpha2Code> eitherCountryCode = Alpha2Code.Create(alpha2CountryCode);
            Either<Error, Option<State>> eitherOptionalState = CreateOptionalState(state);
            Either<Error, City> eitherCity = City.Create(city);
            Either<Error, PostCode> eitherPostCode = PostCode.Create(postCode);
            Either<Error, Address> eitherAddress = Address.Create(address);

            Either<Error, (Alpha2Code countryCode, Option<State> state, City city, PostCode postCode, Address address)> combined =
               from cc in eitherCountryCode
               from os in eitherOptionalState
               from c in eitherCity
               from pc in eitherPostCode
               from a in eitherAddress
               select (cc, os, c, pc, a);

            return
                combined.Map(
                    combined =>
                        new LocationDetails(
                            combined.countryCode,
                            combined.state,
                            combined.city,
                            combined.postCode,
                            combined.address));
        }

        private static Either<Error, Option<State>> CreateOptionalState(string state)
            =>
                string.IsNullOrWhiteSpace(state)
                    ? Right(Option<State>.None)             // If string's empty it means there's no state 
                    : ValueObjects.State.Create(state)      // If not empty, then we try to create the state and then assign the final result based on the Create outcome
                        .Right(value => Right<Error, Option<State>>(Some(value)))
                        .Left(value => Left<Error, Option<State>>(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<LocationDetails>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] LocationDetails other)
            =>
                ValueObjectComparer<LocationDetails>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<LocationDetails>.Instance.GetHashCode();

        public static bool operator ==(LocationDetails left, LocationDetails right)
            =>
                ValueObjectComparer<LocationDetails>.Instance.Equals(left, right);
        public static bool operator !=(LocationDetails left, LocationDetails right)
            =>
                !(left == right);
    }
}
