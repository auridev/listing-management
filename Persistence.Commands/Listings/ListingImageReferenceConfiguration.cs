using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers;

namespace Persistence.Commands.Listings
{
    public class ListingImageReferenceConfiguration : IEntityTypeConfiguration<ListingImageReference>
    {
        public void Configure(EntityTypeBuilder<ListingImageReference> builder)
        {
            builder
                .ToTable("listing_image_references")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("id")
                .IsRequired(true);

            builder
                .Property(p => p.ParentReference)
                .HasColumnName("parent_reference")
                .IsRequired(true);

            builder
                .Property(p => p.FileName)
                .HasColumnName("file_name")
                .HasMaxLength(100)
                .HasConversion(domain => domain.Value, db => FileName.Create(db).ToUnsafeRight())
                .IsRequired(true);

            builder
                .Property(p => p.FileSize)
                .HasColumnName("file_size")
                .HasConversion(domain => domain.Bytes, db => FileSize.Create(db).ToUnsafeRight())
                .IsRequired(true);
        }
    }
}
