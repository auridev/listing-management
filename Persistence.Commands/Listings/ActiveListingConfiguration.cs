using Core.Domain.Listings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Listings
{
    public class ActiveListingConfiguration : ListingConfiguration<ActiveListing>
    {
        public override void Configure(EntityTypeBuilder<ActiveListing> builder)
        {
            base.Configure(builder);

            builder
                .ToTable("active_listings")
                .HasKey(p => p.Id);

            builder
                .HasIndex(p => p.Owner)
                .HasDatabaseName("index_active_listing_owner");

            builder
                .Property(p => p.ExpirationDate)
                .HasColumnName("expiration_date")
                .IsRequired();

            builder
                .HasMany(p => p.ActiveOffers)
                .WithOne()
                .HasForeignKey("listing_id")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);


            builder
                .OwnsMany(
                    p => p.Leads,
                    lead =>
                    {
                        lead.ToTable("leads");
                        lead.Property<long>("lead_id");
                        lead.HasKey("lead_id");

                        lead
                            .Property(p => p.UserInterested)
                            .HasColumnName("user_interested")
                            .HasConversion(new OwnerConverter())
                            .IsRequired();

                        lead
                            .Property(p => p.DetailsSeenOn)
                            .HasColumnName("details_seen_on")
                            .IsRequired();

                        lead
                            .WithOwner()
                            .HasForeignKey("active_listing_id");
                    });

            builder
                .OwnsMany(
                    p => p.Favorites,
                    favorite =>
                    {
                        favorite.ToTable("favorites");
                        favorite.Property<long>("favorite_id");
                        favorite.HasKey("favorite_id");

                        favorite
                            .Property(p => p.FavoredBy)
                            .HasColumnName("favored_by")
                            .HasConversion(new OwnerConverter())
                            .IsRequired();

                        favorite
                            .Property(p => p.MarkedAsFavoriteOn)
                            .HasColumnName("marked_as_favorite_on")
                            .IsRequired();

                        favorite
                            .WithOwner()
                            .HasForeignKey("active_listing_id");
                    });
        }
    }
}
