using Core.Domain.Listings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Listings
{
    public class SuspiciousListingConfiguration : ListingConfiguration<SuspiciousListing>
    {
        public override void Configure(EntityTypeBuilder<SuspiciousListing> builder)
        {
            base.Configure(builder);

            builder
                .ToTable("suspicious_listings")
                .HasKey(p => p.Id);

            builder
                .HasIndex(p => p.Owner)
                .HasDatabaseName("index_suspicious_listing_owner");

            builder
                .Property(p => p.Reason)
                .HasColumnName("reason")
                .HasMaxLength(500)
                .HasConversion(new TrimmedStringConverter())
                .IsRequired();
        }
    }
}
