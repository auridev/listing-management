using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

namespace Persistence.Commands.Listings
{
    public class ImageReferenceConfiguration : IEntityTypeConfiguration<ImageReference>
    {
        public void Configure(EntityTypeBuilder<ImageReference> builder)
        {
            builder
                .ToTable("image_references")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("id")
                .IsRequired();

            builder
                .Property(p => p.ParentReference)
                .HasColumnName("parent_reference")
                .IsRequired();

            builder
                .Property(p => p.FileName)
                .HasColumnName("file_name")
                .HasMaxLength(100)
                .HasConversion(new FileNameConverter())
                .IsRequired();

            builder
                .Property(p => p.FileSize)
                .HasColumnName("file_size")
                .HasConversion(new FileSizeConverter())
                .IsRequired();
        }
    }
}
