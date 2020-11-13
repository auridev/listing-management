using Core.Domain.Common;
using Core.Domain.Listings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Listings
{
    public class SuspiciousListingConfiguration : IEntityTypeConfiguration<SuspiciousListing>
    {
        public void Configure(EntityTypeBuilder<SuspiciousListing> builder)
        {
            builder
                .ToTable("suspicious_listings")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("id");

            builder
                .Property(p => p.Owner)
                .HasColumnName("owner")
                .HasConversion(domain => domain.UserId, db => Owner.Create(db))
                .IsRequired(true);

            builder.OwnsOne(
                p => p.ListingDetails,
                listingDetails =>
                {
                    listingDetails
                        .Property(p => p.Title)
                        .HasColumnName("title")
                        .HasMaxLength(50)
                        .HasConversion(domain => domain.Value.Value, db => Title.Create(db));

                    listingDetails
                       .Property(p => p.MaterialType)
                       .HasColumnName("material_type_id")
                       .HasMaxLength(2)
                       .HasConversion(domain => domain.Id, db => MaterialType.ById(db));

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
                                .HasConversion(domain => domain.Symbol, db => MassMeasurementUnit.BySymbol(db));
                        });

                    listingDetails
                        .Property(p => p.Description)
                        .HasColumnName("description")
                        .HasMaxLength(500)
                        .HasConversion(domain => domain.Value.Value, db => Description.Create(db));
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
                                .HasConversion(domain => domain.Value, db => TrimmedString.Create(db));

                            personName
                                .Property(p => p.LastName)
                                .HasColumnName("last_name")
                                .HasMaxLength(50)
                                .HasConversion(domain => domain.Value, db => TrimmedString.Create(db));
                        });

                    contactDetails
                        .Property(p => p.Phone)
                        .HasColumnName("phone_number")
                        .HasMaxLength(25)
                        .HasConversion(domain => domain.Number, db => Phone.Create(db));

                    contactDetails
                        .Property(p => p.___efCoreCompany)
                        .HasColumnName("company")
                        .HasMaxLength(50)
                        .HasConversion(domain => domain.Name.Value, db => Company.Create(db));
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
                        .HasConversion(domain => domain.Value, db => Alpha2Code.Create(db));

                     locationDetails
                        .Property(p => p.City)
                        .HasColumnName("city")
                        .HasMaxLength(50)
                        .HasConversion(domain => domain.Name.Value, db => City.Create(db));

                     locationDetails
                        .Property(p => p.PostCode)
                        .HasColumnName("post_code")
                        .HasMaxLength(15)
                        .HasConversion(domain => domain.Value.Value, db => PostCode.Create(db));

                     locationDetails
                        .Property(p => p.Address)
                        .HasColumnName("address")
                        .HasMaxLength(250)
                        .HasConversion(domain => domain.Value.Value, db => Address.Create(db));

                     locationDetails
                       .Property(p => p.___efCoreState)
                       .HasColumnName("state")
                       .HasMaxLength(25)
                       .HasConversion(domain => domain.Name.Value, db => State.Create(db));
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
                .Property(p => p.MarkedAsSuspiciousAt)
                .HasColumnName("marked_as_suspicious_at")
                .IsRequired(true);

            builder
                .Property(p => p.Reason)
                .HasColumnName("reason")
                .HasMaxLength(500)
                .HasConversion(domain => domain.Value, db => TrimmedString.Create(db))
                .IsRequired(true);
        }
    }
}
