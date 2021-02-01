using Core.Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Offers
{
    public class ActiveOfferConfiguration : OfferConfiguration<ActiveOffer>
    {
        public override void Configure(EntityTypeBuilder<ActiveOffer> builder)
        {
            base.Configure(builder);

            builder
                .ToTable("active_offers")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.___efCoreSeenDate)
                .HasColumnName("seen_date")
                .HasConversion(new SeenDateConverter());
              builder
                .Ignore(b => b.SeenDate);
        }
    }
}
