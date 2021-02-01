using Core.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Profiles
{
    public class PassiveProfileConfiguration : ProfileConfiguration<PassiveProfile>
    {
        public override void Configure(EntityTypeBuilder<PassiveProfile> builder)
        {
            base.Configure(builder);

            builder
                .ToTable("passive_profiles")
                .HasKey(p => p.Id);

            builder
                .HasIndex(p => p.UserId)
                .HasDatabaseName("index_passive_profile_user_id");

            builder
                .Property(p => p.DeactivationDate)
                .HasColumnName("deactivation_date")
                .IsRequired();

            builder
                .Property(p => p.Reason)
                .HasColumnName("deactivation_reason")
                .HasMaxLength(500)
                .HasConversion(new TrimmedStringConverter())
                .IsRequired();
        }
    }
}
