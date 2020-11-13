using Core.Domain.Common;
using Core.Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Offers
{
    public class RejectedOfferConfiguration : IEntityTypeConfiguration<RejectedOffer>
    {
        public void Configure(EntityTypeBuilder<RejectedOffer> builder)
        {
            builder
                .ToTable("rejected_offers")
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
                p => p.MonetaryValue,
                monetaryValue =>
                {
                    monetaryValue
                        .Property(p => p.Value)
                        .HasColumnName("monetary_value")
                        .HasColumnType("money")
                        .IsRequired(true);

                    monetaryValue
                        .Property(p => p.CurrencyCode)
                        .HasColumnName("currency_code")
                        .HasMaxLength(3)
                        .HasConversion(domain => domain.Value, db => CurrencyCode.Create(db))
                        .IsRequired(true);
                });

            builder
                .Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired(true);
        }
    }
}
