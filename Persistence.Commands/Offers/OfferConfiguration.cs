using Core.Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Offers
{
    public class OfferConfiguration<T> : IEntityTypeConfiguration<T> where T : Offer
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
                p => p.MonetaryValue,
                monetaryValue =>
                {
                    monetaryValue
                        .Property(p => p.Value)
                        .HasColumnName("monetary_value")
                        .HasColumnType("money")
                        .IsRequired();

                    monetaryValue
                        .Property(p => p.CurrencyCode)
                        .HasColumnName("currency_code")
                        .HasMaxLength(3)
                        .HasConversion(new CurrencyCodeConverter())
                        .IsRequired();
                });

            builder
                .Navigation(p => p.MonetaryValue)
                .IsRequired();

            builder
                .Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired();
        }
    }
}
