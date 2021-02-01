using Core.Domain.Listings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Listings
{
    public class ListingConfiguration<T> : IEntityTypeConfiguration<T> where T : Listing
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .Property(p => p.Id)
                .HasColumnName("id");

            builder
                .Property(p => p.Owner)
                .HasColumnName("owner")
                .HasConversion(new OwnerConverter())
                .IsRequired();

            builder.OwnsOne(
                p => p.ListingDetails,
                listingDetails =>
                {
                    listingDetails
                        .Property(p => p.Title)
                        .HasColumnName("title")
                        .HasMaxLength(50)
                        .HasConversion(new TitleConverter())
                        .IsRequired();

                    listingDetails
                       .Property(p => p.MaterialType)
                       .HasColumnName("material_type_id")
                       .HasConversion(new MaterialTypeConverter())
                       .IsRequired();

                    listingDetails.OwnsOne(
                        p => p.Weight,
                        weight =>
                        {
                            weight
                                .Property(p => p.Value)
                                .HasColumnName("weight_value")
                                .IsRequired();

                            weight
                                .Property(p => p.Unit)
                                .HasColumnName("weight_unit")
                                .HasMaxLength(2)
                                .HasConversion(new MassMeasurementUnitConverter())
                                .IsRequired();
                        });

                    listingDetails
                        .Navigation(p => p.Weight)
                        .IsRequired();

                    listingDetails
                        .Property(p => p.Description)
                        .HasColumnName("description")
                        .HasMaxLength(500)
                        .HasConversion(new DescriptionConverter())
                        .IsRequired();
                });

            builder
                .Navigation(p => p.ListingDetails)
                .IsRequired();

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
                                .HasConversion(new TrimmedStringConverter())
                                .IsRequired();

                            personName
                                .Property(p => p.LastName)
                                .HasColumnName("last_name")
                                .HasMaxLength(50)
                                .HasConversion(new TrimmedStringConverter())
                                .IsRequired();
                        });

                    contactDetails
                        .Navigation(p => p.PersonName)
                        .IsRequired();

                    contactDetails
                        .Property(p => p.Phone)
                        .HasColumnName("phone_number")
                        .HasMaxLength(25)
                        .HasConversion(new PhoneConverter())
                        .IsRequired();

                    contactDetails
                        .Property(p => p.___efCoreCompany)
                        .HasColumnName("company")
                        .HasMaxLength(50)
                        .HasConversion(new CompanyConverter());
                    contactDetails
                        .Ignore(cd => cd.Company);
                });

            builder
                .Navigation(p => p.ContactDetails)
                .IsRequired();


            builder.OwnsOne(
                 p => p.LocationDetails,
                 locationDetails =>
                 {
                     locationDetails
                        .Property(p => p.CountryCode)
                        .HasColumnName("country_code")
                        .HasMaxLength(5)
                        .HasConversion(new Alpha2CodeConverter())
                        .IsRequired();

                     locationDetails
                        .Property(p => p.City)
                        .HasColumnName("city")
                        .HasMaxLength(50)
                        .HasConversion(new CityConverter())
                        .IsRequired();

                     locationDetails
                        .Property(p => p.PostCode)
                        .HasColumnName("post_code")
                        .HasMaxLength(15)
                        .HasConversion(new PostCodeConverter())
                        .IsRequired();

                     locationDetails
                        .Property(p => p.Address)
                        .HasColumnName("address")
                        .HasMaxLength(250)
                        .HasConversion(new AddressConverter())
                        .IsRequired();

                     locationDetails
                       .Property(p => p.___efCoreState)
                       .HasColumnName("state")
                       .HasMaxLength(25)
                       .HasConversion(new StateConverter());
                     locationDetails
                        .Ignore(ld => ld.State);

                 });

            builder
                .Navigation(p => p.LocationDetails)
                .IsRequired();

            builder.OwnsOne(
                p => p.GeographicLocation,
                geographicLocation =>
                {
                    geographicLocation
                        .Property(pp => pp.Latitude)
                        .HasColumnName("latitude")
                        .IsRequired();

                    geographicLocation
                        .Property(pp => pp.Longitude)
                        .HasColumnName("longitude")
                        .IsRequired();
                });

            builder
                .Navigation(p => p.GeographicLocation)
                .IsRequired();

            builder
                .Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired();
        }
    }
}
