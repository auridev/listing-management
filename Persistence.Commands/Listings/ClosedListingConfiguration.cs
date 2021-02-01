using Core.Domain.Listings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Listings
{
    public class ClosedListingConfiguration : ListingConfiguration<ClosedListing>
    {
        public override void Configure(EntityTypeBuilder<ClosedListing> builder)
        {
            base.Configure(builder);

            builder
                .ToTable("closed_listings")
                .HasKey(p => p.Id);

            builder
                .HasIndex(p => p.Owner)
                .HasDatabaseName("index_closed_listing_owner");

            builder
                .HasMany(p => p.ClosedOffers)
                .WithOne()
                .HasForeignKey("listing_id")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(
                 p => p.AcceptedOffer,
                 acceptedOffer =>
                 {
                     acceptedOffer
                        .Property(p => p.Id)
                        .HasColumnName("accepted_offer_id")
                        .IsRequired();

                     acceptedOffer
                        .Property(p => p.Owner)
                        .HasColumnName("accepted_offer_owner")
                        .HasConversion(new OwnerConverter())
                        .IsRequired();

                     acceptedOffer.OwnsOne(
                        p => p.MonetaryValue,
                        monetaryValue =>
                        {
                            monetaryValue
                                .Property(p => p.Value)
                                .HasColumnName("accepted_offer_monetary_value")
                                .HasColumnType("money")
                                .IsRequired();

                            monetaryValue
                                .Property(p => p.CurrencyCode)
                                .HasColumnName("accepted_offer_currency_code")
                                .HasMaxLength(3)
                                .HasConversion(new CurrencyCodeConverter())
                                .IsRequired();
                        });

                     acceptedOffer
                        .Navigation(p => p.MonetaryValue)
                        .IsRequired();

                     acceptedOffer
                         .Property(p => p.CreatedDate)
                         .HasColumnName("accepted_offer_created_date")
                         .IsRequired();
                 });

            builder
                .Navigation(p => p.AcceptedOffer)
                .IsRequired();
        }
    }
}
