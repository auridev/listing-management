using Core.Domain.Listings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Commands.Listings
{
    public class NewListingConfiguration : ListingConfiguration<NewListing>
    {
        public override void Configure(EntityTypeBuilder<NewListing> builder)
        {
            base.Configure(builder);

            builder
                .ToTable("new_listings")
                .HasKey(p => p.Id);

            builder
                .HasIndex(p => p.Owner)
                .HasDatabaseName("index_new_listing_owner");
        }
    }
}
