using MailSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailSystem.Persistence.Configurations;

public class MailboxEntryConfiguration : IEntityTypeConfiguration<MailboxEntry>
{
    public void Configure(EntityTypeBuilder<MailboxEntry> builder)
    {
        builder.ToTable("MailboxEntries");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Message)
            .WithMany(x => x.MailboxEntries)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.MailboxEntries)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.MailboxType, x.ReceivedAt });
        builder.HasIndex(x => new { x.MessageId, x.UserId });

        builder.Property(x => x.IsRead).HasDefaultValue(false);
        builder.Property(x => x.IsStarred).HasDefaultValue(false);
        builder.Property(x => x.IsImportant).HasDefaultValue(false);
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
    }
}