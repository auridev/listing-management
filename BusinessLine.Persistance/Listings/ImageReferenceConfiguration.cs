using BusinessLine.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLine.Persistence.Listings
{
    public class ImageReferenceConfiguration : IEntityTypeConfiguration<ImageReference>
    {
        public void Configure(EntityTypeBuilder<ImageReference> builder)
        {
            throw new NotImplementedException();
        }
    }
}
