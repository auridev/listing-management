using Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Application.UnitTests.TestMocks
{
    internal class ValueObjectMocks
    {
        public static Email Email => Email.Create("peter@home.com");
        public static ContactDetails ContactDetails => ContactDetails.Create(PersonName.Create("peter", "peterson"),
                Company.Create("self emplyoment inc"),
                Phone.Create("+111 222 33333"));
        public static LocationDetails LocationDetails => LocationDetails.Create(
            Alpha2Code.Create("GB"),
            State.Create("London"),
            City.Create("London"),
            PostCode.Create("aaa1"),
            Address.Create("some random place 12"));
        public static GeographicLocation GeographicLocation => GeographicLocation.Create(10D, 10D);
        public static UserPreferences UserPreferences => UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("GBP"));
        public static ListingDetails ListingDetails => ListingDetails.Create(
            Title.Create("very nice listing"),
            MaterialType.Electronics,
            Weight.Create(1.2F, MassMeasurementUnit.Kilogram),
            Description.Create("very very nice"));


        public static readonly Recipient Recipient = Recipient.Create(Guid.Parse("d1633a55-34fa-4e78-b2c0-348c3951d358"));
        public static readonly Subject Subject = Subject.Create("Some important stuff");
        public static readonly MessageBody MessageBody = MessageBody.Create("Hey Vladimir, pay attention");
    }
}
