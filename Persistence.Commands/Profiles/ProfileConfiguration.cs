using Core.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Profiles
{
    public class ProfileConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Profile
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .Property(p => p.Id)
                .HasColumnName("id");

            builder
                .Property(p => p.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder
                .Property(p => p.Email)
                .HasColumnName("email")
                .HasMaxLength(50)
                .HasConversion(new EmailConverter())
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

            builder.OwnsOne(
                 p => p.UserPreferences,
                 userPreferences =>
                 {
                     userPreferences
                        .Property(p => p.DistanceUnit)
                        .HasColumnName("distance_unit")
                        .HasMaxLength(2)
                        .HasConversion(new DistanceMeasurementUnitConverter())
                        .IsRequired();

                     userPreferences
                        .Property(p => p.MassUnit)
                        .HasColumnName("mass_unit")
                        .HasMaxLength(2)
                        .HasConversion(new MassMeasurementUnitConverter())
                        .IsRequired();

                     userPreferences
                        .Property(p => p.CurrencyCode)
                        .HasColumnName("currency_code")
                        .HasMaxLength(3)
                        .HasConversion(new CurrencyCodeConverter())
                        .IsRequired();
                 });

            builder
                .Navigation(p => p.UserPreferences)
                .IsRequired();

            builder
                .Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired();
        }
    }
}
