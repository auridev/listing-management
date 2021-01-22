﻿using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers;

namespace Persistence.Commands.Listings
{
    public class ActiveListingConfiguration : IEntityTypeConfiguration<ActiveListing>
    {
        public void Configure(EntityTypeBuilder<ActiveListing> builder)
        {
            builder
                .ToTable("active_listings")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("id");

            builder
                .HasIndex(p => p.Owner)
                .HasDatabaseName("index_active_listing_owner");

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
                .Property(p => p.ExpirationDate)
                .HasColumnName("expiration_date")
                .IsRequired(true);

            builder
                .HasMany(p => p.Offers)
                .WithOne()
                .HasForeignKey("active_listing_id")
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder
                .OwnsMany(
                    p => p.Leads,
                    lead =>
                    {
                        lead.ToTable("leads");
                        lead.Property<long>("lead_id");
                        lead.HasKey("lead_id");

                        lead
                            .Property(p => p.UserInterested)
                            .HasColumnName("user_interested")
                            .HasConversion(domain => domain.UserId, db => Owner.Create(db).ToUnsafeRight())
                            .IsRequired(true);

                        lead
                            .Property(p => p.DetailsSeenOn)
                            .HasColumnName("details_seen_on")
                            .IsRequired(true);

                        lead
                            .WithOwner()
                            .HasForeignKey("active_listing_id");
                    });

            builder
                .OwnsMany(
                    p => p.Favorites,
                    favorite =>
                    {
                        favorite.ToTable("favorites");
                        favorite.Property<long>("favorite_id");
                        favorite.HasKey("favorite_id");

                        favorite
                            .Property(p => p.FavoredBy)
                            .HasColumnName("favored_by")
                            .HasConversion(domain => domain.UserId, db => Owner.Create(db).ToUnsafeRight())
                            .IsRequired(true);

                        favorite
                            .Property(p => p.MarkedAsFavoriteOn)
                            .HasColumnName("marked_as_favorite_on")
                            .IsRequired(true);

                        favorite
                            .WithOwner()
                            .HasForeignKey("active_listing_id");
                    });
        }
    }
}
