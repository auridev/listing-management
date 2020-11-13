using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
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
                    .Some(v => { 
                        ___efCoreState = v; 
                    })
                    .None(() => { 
                        ___efCoreState = null; 
                    });
            }
        }

        private LocationDetails() { }

        private LocationDetails(Alpha2Code countryCode, Option<State> state, City city, PostCode postCode, Address address)
        {
            CountryCode = countryCode;
            State = state;
            City = city;
            PostCode = postCode;
            Address = address;
        }

        public static LocationDetails Create(
            Alpha2Code countryCode,
            Option<State> state, 
            City city, 
            PostCode postCode, 
            Address address)
            => new LocationDetails(countryCode, state, city, postCode, address);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<LocationDetails>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] LocationDetails other)
            => ValueObjectComparer<LocationDetails>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<LocationDetails>.Instance.GetHashCode();

        public static bool operator ==(LocationDetails left, LocationDetails right)
            => ValueObjectComparer<LocationDetails>.Instance.Equals(left, right);
        public static bool operator !=(LocationDetails left, LocationDetails right)
            => !(left == right);
    }
}
