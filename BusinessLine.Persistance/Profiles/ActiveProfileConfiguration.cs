using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessLine.Persistence.Profiles
{
    public class ActiveProfileConfiguration : IEntityTypeConfiguration<ActiveProfile>
    {
        public void Configure(EntityTypeBuilder<ActiveProfile> builder)
        {
            builder
                .ToTable("active_profiles")
                .HasKey(p => p.Id);
            builder
                .Property(p => p.Id)
                .HasColumnName("id");
            builder
                .Property(p => p.UserId)
                .HasColumnName("user_id")
                .IsRequired(true);

            builder
                .Property(p => p.Email)
                .HasColumnName("email")
                .HasMaxLength(50)
                .HasConversion(domain => domain.Value, db => Email.Create(db))
                .IsRequired(true);

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
                        .Property(p => p.Company)
                        .HasColumnName("company")
                        .HasMaxLength(50)
                        .HasConversion(domain => domain.Name.Value, db => Company.Create(db));

                    contactDetails
                        .Property(p => p.Phone)
                        .HasColumnName("phone_number")
                        .HasMaxLength(25)
                        .HasConversion(domain => domain.Number, db => Phone.Create(db));
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
                        .Property(p => p.State)
                        .HasColumnName("state")
                        .HasMaxLength(25)
                        .HasConversion(domain => domain.Name.Value, db => State.Create(db));

                     locationDetails
                        .Property(p => p.City)
                        .HasColumnName("city")
                        .HasMaxLength(25)
                        .HasConversion(domain => domain.Name.Value, db => City.Create(db));

                     locationDetails
                        .Property(p => p.PostCode)
                        .HasColumnName("post_code")
                        .HasMaxLength(10)
                        .HasConversion(domain => domain.Value.Value, db => PostCode.Create(db));

                     locationDetails
                        .Property(p => p.Address)
                        .HasColumnName("address")
                        .HasMaxLength(100)
                        .HasConversion(domain => domain.Value.Value, db => Address.Create(db));
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

            builder.OwnsOne(
                 p => p.UserPreferences,
                 userPreferences =>
                 {
                     userPreferences
                        .Property(p => p.DistanceUnit)
                        .HasColumnName("distance_unit")
                        .HasMaxLength(2)
                        .HasConversion(domain => domain.Symbol, db => DistanceMeasurementUnit.BySymbol(db));

                     userPreferences
                        .Property(p => p.MassUnit)
                        .HasColumnName("mass_unit")
                        .HasMaxLength(2)
                        .HasConversion(domain => domain.Symbol, db => MassMeasurementUnit.BySymbol(db));

                     userPreferences
                        .Property(p => p.CurrencyCode)
                        .HasColumnName("currency_code")
                        .HasMaxLength(3)
                        .HasConversion(domain => domain.Value, db => CurrencyCode.Create(db));
                 });

            builder
                .Property(p => p.IntroductionSeenOn)
                .HasColumnName("introduction_seen_on")
                .HasConversion(p => p.Value, p => SeenDate.Create(p))
                .IsRequired(true);
        }
    }
}
