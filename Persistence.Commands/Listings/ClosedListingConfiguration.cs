﻿using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers;

namespace Persistence.Commands.Listings
{
    public class ClosedListingConfiguration : IEntityTypeConfiguration<ClosedListing>
    {
        public void Configure(EntityTypeBuilder<ClosedListing> builder)
        {
            builder
                .ToTable("closed_listings")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("id");

            builder
                .HasIndex(p => p.Owner)
                .HasDatabaseName("index_closed_listing_owner");

            builder
                .Property(p => p.Owner)
                .HasColumnName("owner")
                .HasConversion(domain => domain.UserId, db => Owner.Create(db).ToUnsafeRight())
                .IsRequired(true);

            builder.OwnsOne(
                p => p.ListingDetails,
                listingDetails =>
                {
                    listingDetails
                        .Property(p => p.Title)
                        .HasColumnName("title")
                        .HasMaxLength(50)
                        .HasConversion(domain => domain.Value.Value, db => Title.Create(db).ToUnsafeRight());

                    listingDetails
                       .Property(p => p.MaterialType)
                       .HasColumnName("material_type_id")
                       .HasConversion(domain => domain.Id, db => MaterialType.ById(db).ToUnsafeRight());

                    listingDetails.OwnsOne(
                        p => p.Weight,
                        weight =>
                        {
                            weight
                                .Property(p => p.Value)
                                .HasColumnName("weight_value");

                            weight
                                .Property(p => p.Unit)
                                .HasColumnName("weight_unit")
                                .HasMaxLength(2)
                                .HasConversion(domain => domain.Symbol, db => MassMeasurementUnit.BySymbol(db).ToUnsafeRight());
                        });

                    listingDetails
                        .Property(p => p.Description)
                        .HasColumnName("description")
                        .HasMaxLength(500)
                        .HasConversion(domain => domain.Value.Value, db => Description.Create(db).ToUnsafeRight());
                });


            builder.OwnsOne(
                p => p.ContactDetails,
                contactDetails =>
                {
                    contactDetails.OwnsOne(
                        p => p.PersonName,
                        personName =>
                        {
                            personName
                                .Property(p => p.FirstName)
                                .HasColumnName("first_name")
                                .HasMaxLength(50)
                                .HasConversion(domain => domain.Value, db => TrimmedString.Create(db).ToUnsafeRight());

                            personName
                                .Property(p => p.LastName)
                                .HasColumnName("last_name")
                                .HasMaxLength(50)
                                .HasConversion(domain => domain.Value, db => TrimmedString.Create(db).ToUnsafeRight());
                        });

                    contactDetails
                        .Property(p => p.Phone)
                        .HasColumnName("phone_number")
                        .HasMaxLength(25)
                        .HasConversion(domain => domain.Number, db => Phone.Create(db).ToUnsafeRight());

                    contactDetails
                        .Property(p => p.___efCoreCompany)
                        .HasColumnName("company")
                        .HasMaxLength(50)
                        .HasConversion(domain => domain.Name.Value, db => Company.Create(db).ToUnsafeRight());
                    contactDetails
                        .Ignore(cd => cd.Company);
                });

            builder.OwnsOne(
                 p => p.LocationDetails,
                 locationDetails =>
                 {
                     locationDetails
                        .Property(p => p.CountryCode)
                        .HasColumnName("country_code")
                        .HasMaxLength(5)
                        .HasConversion(domain => domain.Value, db => Alpha2Code.Create(db).ToUnsafeRight());

                     locationDetails
                        .Property(p => p.City)
                        .HasColumnName("city")
                        .HasMaxLength(50)
                        .HasConversion(domain => domain.Name.Value, db => City.Create(db).ToUnsafeRight());

                     locationDetails
                        .Property(p => p.PostCode)
                        .HasColumnName("post_code")
                        .HasMaxLength(15)
                        .HasConversion(domain => domain.Value.Value, db => PostCode.Create(db).ToUnsafeRight());

                     locationDetails
                        .Property(p => p.Address)
                        .HasColumnName("address")
                        .HasMaxLength(250)
                        .HasConversion(domain => domain.Value.Value, db => Address.Create(db).ToUnsafeRight());

                     locationDetails
                       .Property(p => p.___efCoreState)
                       .HasColumnName("state")
                       .HasMaxLength(25)
                       .HasConversion(domain => domain.Name.Value, db => State.Create(db).ToUnsafeRight());
                     locationDetails
                        .Ignore(ld => ld.State);
                 });

            builder.OwnsOne(
                p => p.GeographicLocation,
                geographicLocation =>
                {
                    geographicLocation
                        .Property(pp => pp.Latitude)
                        .HasColumnName("latitude");

                    geographicLocation
                        .Property(pp => pp.Longitude)
                        .HasColumnName("longitude");
                });

            builder
                .Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired(true);

            builder
                .Property(p => p.ClosedOn)
                .HasColumnName("closed_on")
                .IsRequired(true);

            builder
                .HasMany(p => p.RejectedOffers)
                .WithOne()
                .HasForeignKey("closed_listing_id")
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(
                 p => p.AcceptedOffer,
                 acceptedOffer =>
                 {
                     acceptedOffer
                        .Property(p => p.Id)
                        .HasColumnName("accepted_offer_id");

                     acceptedOffer
                        .Property(p => p.Owner)
                        .HasColumnName("accepted_offer_owner")
                        .HasConversion(domain => domain.UserId, db => Owner.Create(db).ToUnsafeRight())
                        .IsRequired(true);

                     acceptedOffer.OwnsOne(
                        p => p.MonetaryValue,
                        monetaryValue =>
                        {
                            monetaryValue
                                .Property(p => p.Value)
                                .HasColumnName("accepted_offer_monetary_value")
                                .HasColumnType("money")
                                .IsRequired(true);

                            monetaryValue
                                .Property(p => p.CurrencyCode)
                                .HasColumnName("accepted_offer_currency_code")
                                .HasMaxLength(3)
                                .HasConversion(domain => domain.Value, db => CurrencyCode.Create(db).ToUnsafeRight())
                                .IsRequired(true);
                        });

                     acceptedOffer
                         .Property(p => p.CreatedDate)
                         .HasColumnName("accepted_offer_created_date")
                         .IsRequired(true);
                 });
        }
    }
}
