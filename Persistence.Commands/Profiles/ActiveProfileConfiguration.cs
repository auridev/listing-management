using Core.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Profiles
{
    public class ActiveProfileConfiguration : ProfileConfiguration<ActiveProfile>
    {
        public override void Configure(EntityTypeBuilder<ActiveProfile> builder)
        {
            base.Configure(builder);

            builder
                .ToTable("active_profiles")
                .HasKey(p => p.Id);

            builder
                .HasIndex(p => p.UserId)
                .HasDatabaseName("index_active_profile_user_id");

            builder
                .Property(p => p.___efCoreSeenDate)
                .HasColumnName("introduction_seen_on")
                .HasConversion(new SeenDateConverter());
            builder
                .Ignore(b => b.IntroductionSeenOn);
        }
    }
}
