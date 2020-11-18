using Core.Domain.Common;
using Core.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Commands.Messages
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .ToTable("messages")
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("id");

            builder
                .Property(p => p.Recipient)
                .HasColumnName("recipient")
                .HasConversion(domain => domain.UserId, db => Recipient.Create(db))
                .IsRequired(true);

            builder
                .Property(p => p.Subject)
                .HasColumnName("subject")
                .HasMaxLength(200)
                .HasConversion(domain => domain.Value.Value, db => Subject.Create(db))
                .IsRequired(true);

            builder
                .Property(p => p.Body)
                .HasColumnName("body")
                .HasMaxLength(1000)
                .HasConversion(domain => domain.Content, db => MessageBody.Create(db))
                .IsRequired(true);

            builder
                .Property(p => p.___efCoreSeenDate)
                .HasColumnName("seen_date")
                .HasConversion(domain => domain.Value, db => SeenDate.Create(db))
                .IsRequired(false);
            builder
                .Ignore(b => b.SeenDate);

            builder
                .Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired(true);
        }
    }
}
