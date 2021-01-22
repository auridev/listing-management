using Core.Domain.Listings;
using Core.Domain.Messages;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Persistence.Commands.Helpers
{
    public static class SeedData
    {
        public static class Personas
        {
            public static class Peter
            {
                public static Guid UserId
                    => Guid.Parse("626d6839-ca59-405d-979a-b7bec2bf350f");
                public static Email Email
                    => Email.Create("peter@peterson.com").ToUnsafeRight();
                public static ContactDetails ContactDetails
                    => ContactDetails.Create("Peter", "Peterson", "Google", "+333 100 10001").ToUnsafeRight();

                public static LocationDetails LocationDetails
                    => LocationDetails.Create("LT", string.Empty, "vilnius", "11200", "Mindaugo 9").ToUnsafeRight();
                public static GeographicLocation GeographicLocation
                    => GeographicLocation.Create(5.5D, 30.1D).ToUnsafeRight();
                public static ActiveProfile ActiveProfile =>
                    new ActiveProfile(
                        Guid.Parse("684790a7-b883-4f81-9721-e56b4b361227"),
                        UserId,
                        Email,
                        ContactDetails,
                        LocationDetails,
                        GeographicLocation,
                        UserPreferences.Create("km", "kg", "EUR").ToUnsafeRight(),
                        DateTimeOffset.UtcNow.AddDays(-10));
            }

            public static class John
            {
                public static Guid UserId
                    => Guid.Parse("14f2485a-e295-4f73-82a5-15339f08498d");
                public static Email Email
                    => Email.Create("john@johnson.com").ToUnsafeRight();
                public static ContactDetails ContactDetails
                    => ContactDetails.Create("John", "Johnson", string.Empty, "+333 100 20002").ToUnsafeRight();
                public static LocationDetails LocationDetails
                    => LocationDetails.Create("LT", string.Empty, "Kaunas", "22200", "Savanoriu 23.").ToUnsafeRight();
                public static GeographicLocation GeographicLocation
                    => GeographicLocation.Create(10.5D, 45.1D).ToUnsafeRight();
                public static ActiveProfile ActiveProfile
                    => new ActiveProfile(
                        Guid.Parse("7e22dafe-48ab-4de7-990f-4854e06b6a51"),
                        UserId,
                        Email,
                        ContactDetails,
                        LocationDetails,
                        GeographicLocation,
                        UserPreferences.Create("km", "kg", "EUR").ToUnsafeRight(),
                        DateTimeOffset.UtcNow.AddDays(-20));
            }

            public static class Alice
            {
                public static Guid UserId
                    => Guid.Parse("429d32d1-b64c-421e-b013-3d3932b9a654");
                public static Email Email
                    => Email.Create("alice@alison.com").ToUnsafeRight();
                public static ContactDetails ContactDetails
                    => ContactDetails.Create("Alice", "Alison", "Microsoft", "+333 100 30003").ToUnsafeRight();
                public static LocationDetails LocationDetails
                    => LocationDetails.Create("LT", "Klaipedos rajonas", "Klaipeda", "33200", "Juros 18-45").ToUnsafeRight();
                public static GeographicLocation GeographicLocation
                    => GeographicLocation.Create(3.5D, 58.1D).ToUnsafeRight();
                public static ActiveProfile ActiveProfile
                    => new ActiveProfile(
                        Guid.Parse("d86c576f-2c0a-4937-a0f0-a8c5843f04c5"),
                        UserId,
                        Email,
                        ContactDetails,
                        LocationDetails,
                        GeographicLocation,
                        UserPreferences.Create("m", "lb", "USD").ToUnsafeRight(),
                        DateTimeOffset.UtcNow.AddDays(-30));
            }

            public static class Mark
            {
                public static Guid UserId
                    => Guid.Parse("843328f5-228f-419b-a93c-ad0ae075f19d");
                public static Email Email
                    => Email.Create("mark@markeson.com").ToUnsafeRight();
                public static ContactDetails ContactDetails
                    => ContactDetails.Create("Mark", "Markeson", "Netflix", "+333 100 40004").ToUnsafeRight();
                public static LocationDetails LocationDetails
                    => LocationDetails.Create("LT", string.Empty, "Siauliai", "33111", "Kareiviu 89").ToUnsafeRight();
                public static GeographicLocation GeographicLocation
                    => GeographicLocation.Create(10D, 20D).ToUnsafeRight();
                public static PassiveProfile PassiveProfile
                    => new PassiveProfile(
                        Guid.Parse("04d1a08e-7c45-4748-a32c-5527bf67c003"),
                        UserId,
                        Email,
                        ContactDetails,
                        LocationDetails,
                        GeographicLocation,
                        UserPreferences.Create("km", "lb", "USD").ToUnsafeRight(),
                        DateTimeOffset.UtcNow.AddDays(-50),
                        new DateTimeOffset(2020, 12, 1, 13, 30, 35, TimeSpan.FromMinutes(60)),
                        TrimmedString.Create("Criminal activity found").ToUnsafeRight());
            }

            public static class Philip
            {
                public static Guid UserId
                    => Guid.Parse("3618e770-101a-4bcd-9842-cf65c7e7a527");
                public static Email Email
                    => Email.Create("philip@philipson.com").ToUnsafeRight();
                public static ContactDetails ContactDetails
                    => ContactDetails.Create("Philip", "philipson", string.Empty, "+333 100 50005").ToUnsafeRight();
                public static LocationDetails LocationDetails
                    => LocationDetails.Create("LT", string.Empty, "Vilnius", "40000", "Gedimino prs 15").ToUnsafeRight();
                public static GeographicLocation GeographicLocation
                    => GeographicLocation.Create(34D, 17D).ToUnsafeRight();
                public static PassiveProfile PassiveProfile
                    => new PassiveProfile(
                        Guid.Parse("d644e97e-944c-4818-b03b-35c4728d269d"),
                        UserId,
                        Email,
                        ContactDetails,
                        LocationDetails,
                        GeographicLocation,
                        UserPreferences.Create("m", "kg", "EUR").ToUnsafeRight(),
                        DateTimeOffset.UtcNow.AddDays(-60),
                        new DateTimeOffset(2018, 6, 23, 8, 34, 3, TimeSpan.FromMinutes(60)),
                        TrimmedString.Create("No activity").ToUnsafeRight());
            }
        }

        public static class Messages
        {
            public static List<Message> ForPeter => new List<Message>
            {
                new Message(
                    Guid.Parse("bdb6b828-b31c-4d34-9a44-e33fe6d72902"),
                    Recipient.Create(Personas.Peter.UserId).ToUnsafeRight(),
                    Subject.Create("Your listing expired").ToUnsafeRight(),
                    MessageBody.Create("Your first listing has expirted. It won't be visible to others anymore").ToUnsafeRight(),
                    new DateTimeOffset(2020, 12, 1, 00, 00, 00, TimeSpan.FromMinutes(60))),

                new Message(
                    Guid.Parse("93a39c2e-387b-42d6-95bb-abe28803ca79"),
                    Recipient.Create(Personas.Peter.UserId).ToUnsafeRight(),
                    Subject.Create("Your listing has been rejected").ToUnsafeRight(),
                    MessageBody.Create("Your listing has been rejected due to inappropriate wording").ToUnsafeRight(),
                    new DateTimeOffset(2020, 11, 1, 00, 00, 00, TimeSpan.FromMinutes(60))),

                new Message(
                    Guid.Parse("872629f2-cae5-48a5-94e2-a6c8e554776c"),
                    Recipient.Create(Personas.Peter.UserId).ToUnsafeRight(),
                    Subject.Create("Welcome").ToUnsafeRight(),
                    MessageBody.Create("Welcome my dude").ToUnsafeRight(),
                    new DateTimeOffset(2017, 9, 1, 00, 00, 00, TimeSpan.FromMinutes(60)))
            };

            public static List<Message> ForJohn => new List<Message>
            {
                new Message(
                    Guid.Parse("15a51b55-b524-4d44-b1d8-d58bc6759e59"),
                    Recipient.Create(Personas.John.UserId).ToUnsafeRight(),
                    Subject.Create("Your offer has been accepted").ToUnsafeRight(),
                    MessageBody.Create("Nice. Your offer has been accepted").ToUnsafeRight(),
                    new DateTimeOffset(2017, 9, 1, 00, 00, 00, TimeSpan.FromMinutes(60)))
            };
        }


        public static List<NewListing> NewListings => new List<NewListing>
        {
            new NewListing(
                Guid.Parse("67fffb0a-ee36-4b77-8668-a39cc7759911"),
                Owner.Create(Personas.Peter.UserId).ToUnsafeRight(),
                ListingDetails.Create("Old computers", 60, 5, "kg", "My old PCs. All functional").ToUnsafeRight(),
                Personas.Peter.ContactDetails,
                Personas.Peter.LocationDetails,
                Personas.Peter.GeographicLocation,
                new DateTimeOffset(2020, 6, 12, 0, 0, 0, TimeSpan.FromHours(1))),

             new NewListing(
                Guid.Parse("e94f3753-db88-4f88-9ce8-94a9dc386849"),
                Owner.Create(Personas.Peter.UserId).ToUnsafeRight(),
                ListingDetails.Create("Non working mobile phones", 60, 20, "kg", "Non working phones. All kinds").ToUnsafeRight(),
                Personas.Peter.ContactDetails,
                Personas.Peter.LocationDetails,
                Personas.Peter.GeographicLocation,
                new DateTimeOffset(2020, 6, 13, 0, 0, 0, TimeSpan.FromHours(1))),

              new NewListing(
                Guid.Parse("20263319-3c73-45da-adb1-f5bde2b41a1e"),
                Owner.Create(Personas.Alice.UserId).ToUnsafeRight(),
                ListingDetails.Create("Plastic stuff", 40, 4,  "kg", "Plastic stuff from my garage. Bottles, boxes etc").ToUnsafeRight(),
                Personas.Alice.ContactDetails,
                Personas.Alice.LocationDetails,
                Personas.Alice.GeographicLocation,
                new DateTimeOffset(2019, 3, 13, 0, 0, 0, TimeSpan.FromHours(1)))
        };

        public static List<ActiveListing> ActiveListings = new List<ActiveListing>
        {
            new ActiveListing(
                Guid.Parse("64f9fed7-4d98-4cfe-87ec-00d8046cf7b5"),
                Owner.Create(Personas.Peter.UserId).ToUnsafeRight(),
                ListingDetails.Create("Car tires", 70, 100, "lb", "76 old tires").ToUnsafeRight(),
                Personas.Peter.ContactDetails,
                Personas.Peter.LocationDetails,
                Personas.Peter.GeographicLocation,
                DateTimeOffset.UtcNow.AddDays(-10),
                DateTimeOffset.UtcNow.AddDays(30)),

            new ActiveListing(
                Guid.Parse("cda8250f-cf8f-4aa9-bdb6-9d4e70315e61"),
                Owner.Create(Personas.Peter.UserId).ToUnsafeRight(),
                ListingDetails.Create("Metal", 10, 200, "lb", "Old rusted iron and steel poles").ToUnsafeRight(),
                Personas.Peter.ContactDetails,
                Personas.Peter.LocationDetails,
                Personas.Peter.GeographicLocation,
                DateTimeOffset.UtcNow.AddDays(-9),
                DateTimeOffset.UtcNow.AddDays(30)),

            new ActiveListing(
                Guid.Parse("ff50058e-0967-4bac-b7e0-d128bbb7c4a9"),
                Owner.Create(Personas.Peter.UserId).ToUnsafeRight(),
                ListingDetails.Create("Coper", 20, 24, "kg", "Coper cable").ToUnsafeRight(),
                Personas.Peter.ContactDetails,
                Personas.Peter.LocationDetails,
                Personas.Peter.GeographicLocation,
                DateTimeOffset.UtcNow.AddDays(-8),
                DateTimeOffset.UtcNow.AddDays(30)),

            new ActiveListing(
                Guid.Parse("85e5afbf-5850-42f3-bdeb-004a116b3914"),
                Owner.Create(Personas.Alice.UserId).ToUnsafeRight(),
                ListingDetails.Create("Old clothes", 80, 1000, "kg", "Storage unit full of clothes").ToUnsafeRight(),
                Personas.Alice.ContactDetails,
                Personas.Alice.LocationDetails,
                Personas.Alice.GeographicLocation,
                DateTimeOffset.UtcNow.AddDays(-7),
                DateTimeOffset.UtcNow.AddDays(30))
        };

        public static List<PassiveListing> PassiveListings => new List<PassiveListing>
        {
            new PassiveListing(
                Guid.Parse("7094db38-70ef-4a45-943c-f5b6ccaa7dcd"),
                Owner.Create(Personas.Peter.UserId).ToUnsafeRight(),
                ListingDetails.Create("Junk from my garage", 70, 1.5F, "lb", "tires, calbes etc").ToUnsafeRight(),
                Personas.Peter.ContactDetails,
                Personas.Peter.LocationDetails,
                Personas.Peter.GeographicLocation,
                DateTimeOffset.UtcNow.AddDays(-20),
                new DateTimeOffset(2020, 4, 23, 0, 0, 0, TimeSpan.FromHours(0)),
                TrimmedString.Create("Violates business rules. We don't allow junk to be posted in listings").ToUnsafeRight()),

            new PassiveListing(
                Guid.Parse("b6ae4dd3-6325-4f8e-9c5e-2306450e4bc4"),
                Owner.Create(Personas.Peter.UserId).ToUnsafeRight(),
                ListingDetails.Create("Regrigirators", 60, 65F, "kg", "Non-workig fridges. Should have good spare parts though").ToUnsafeRight(),
                Personas.Peter.ContactDetails,
                Personas.Peter.LocationDetails,
                Personas.Peter.GeographicLocation,
                DateTimeOffset.UtcNow.AddDays(-21),
                new DateTimeOffset(2020, 7, 2, 0, 0, 0, TimeSpan.FromHours(0)),
                TrimmedString.Create("Violates business rules. User complaints received").ToUnsafeRight()),

            new PassiveListing(
                Guid.Parse("6d8e7a01-c9ab-421e-8530-66aea80bc0ee"),
                Owner.Create(Personas.Alice.UserId).ToUnsafeRight(),
                ListingDetails.Create("Underware", 80, 1F, "kg", "Self explanatory").ToUnsafeRight(),
                Personas.Alice.ContactDetails,
                Personas.Alice.LocationDetails,
                Personas.Alice.GeographicLocation,
                DateTimeOffset.UtcNow.AddDays(-22),
                new DateTimeOffset(2020, 11, 1, 0, 0, 0, TimeSpan.FromHours(0)),
                TrimmedString.Create("Violates business rules. Can't sell this shit online").ToUnsafeRight())
        };
    }
}
