using Core.Domain.Common;
using Core.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Commands.Profiles
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
                .Property(p => p.___efCoreSeenDate)
                .HasColumnName("introduction_seen_on")
                .HasConversion(domain => domain.Value, db => SeenDate.Create(db))
                .IsRequired(false);
            builder
                .Ignore(b => b.IntroductionSeenOn);
        }
    }
}
