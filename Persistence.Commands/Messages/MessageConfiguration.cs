using Core.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Commands.Helpers.ValueConverters;

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
                .HasIndex(p => p.Recipient)
                .HasDatabaseName("index_message_recipient");

            builder
                .Property(p => p.Recipient)
                .HasColumnName("recipient")
                .HasConversion(new RecipientConverter())
                .IsRequired();

            builder
                .Property(p => p.Subject)
                .HasColumnName("subject")
                .HasMaxLength(200)
                .HasConversion(new SubjectConverter())
                .IsRequired();

            builder
                .Property(p => p.Body)
                .HasColumnName("body")
                .HasMaxLength(1000)
                .HasConversion(new MessageBodyConverter())
                .IsRequired();

            builder
                .Property(p => p.___efCoreSeenDate)
                .HasColumnName("seen_date")
                .HasConversion(new SeenDateConverter());
            builder
                .Ignore(b => b.SeenDate);

            builder
                .Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired();
        }
    }
}
