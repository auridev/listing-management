using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers;

namespace Persistence.Commands.Profiles
{
    public class PassiveProfileConfiguration : IEntityTypeConfiguration<PassiveProfile>
    {
        public void Configure(EntityTypeBuilder<PassiveProfile> builder)
        {
            builder
                .ToTable("passive_profiles")
                .HasKey(p => p.Id);
            builder
                .Property(p => p.Id)
                .HasColumnName("id");

            builder
                .HasIndex(p => p.UserId)
                .HasDatabaseName("index_passive_profile_user_id");

            builder
                .Property(p => p.UserId)
                .HasColumnName("user_id")
                .IsRequired(true);

            builder
                .Property(p => p.Email)
                .HasColumnName("email")
                .HasMaxLength(50)
                .HasConversion(domain => domain.Value, db => Email.Create(db).ToUnsafeRight())
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

            builder.OwnsOne(
                 p => p.UserPreferences,
                 userPreferences =>
                 {
                     userPreferences
                        .Property(p => p.DistanceUnit)
                        .HasColumnName("distance_unit")
                        .HasMaxLength(2)
                        .HasConversion(domain => domain.Symbol, db => DistanceMeasurementUnit.BySymbol(db).ToUnsafeRight());

                     userPreferences
                        .Property(p => p.MassUnit)
                        .HasColumnName("mass_unit")
                        .HasMaxLength(2)
                        .HasConversion(domain => domain.Symbol, db => MassMeasurementUnit.BySymbol(db).ToUnsafeRight());

                     userPreferences
                        .Property(p => p.CurrencyCode)
                        .HasColumnName("currency_code")
                        .HasMaxLength(3)
                        .HasConversion(domain => domain.Value, db => CurrencyCode.Create(db).ToUnsafeRight());
                 });

            builder
                .Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired(true);

            builder
                .Property(p => p.DeactivationDate)
                .HasColumnName("deactivation_date")
                .IsRequired(true);

            builder
                .Property(p => p.Reason)
                .HasColumnName("deactivation_reason")
                .HasMaxLength(500)
                .HasConversion(domain => domain.Value, db => TrimmedString.Create(db).ToUnsafeRight())
                .IsRequired(true);
        }
    }
}
