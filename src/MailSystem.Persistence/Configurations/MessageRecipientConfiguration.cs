using MailSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailSystem.Persistence.Configurations;

public class MessageRecipientConfiguration : IEntityTypeConfiguration<MessageRecipient>
{
    public void Configure(EntityTypeBuilder<MessageRecipient> builder)
    {
        builder.ToTable("MessageRecipients");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Message)
            .WithMany(x => x.Recipients)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.RecipientUser)
            .WithMany(x => x.ReceivedRecipients)
            .HasForeignKey(x => x.RecipientUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.MessageId);
        builder.HasIndex(x => x.RecipientUserId);
        builder.HasIndex(x => new { x.MessageId, x.RecipientUserId, x.RecipientType });
    }
}