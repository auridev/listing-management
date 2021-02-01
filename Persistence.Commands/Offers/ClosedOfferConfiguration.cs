using Core.Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Commands.Offers
{
    public class ClosedOfferConfiguration : OfferConfiguration<ClosedOffer>
    {
        public override void Configure(EntityTypeBuilder<ClosedOffer> builder)
        {
            base.Configure(builder);

            builder
                .ToTable("closed_offers")
                .HasKey(p => p.Id);
        }
    }
}
