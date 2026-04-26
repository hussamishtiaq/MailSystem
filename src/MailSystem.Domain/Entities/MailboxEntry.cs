using MailSystem.Domain.Common;
using MailSystem.Domain.Enums;

namespace MailSystem.Domain.Entities;

public class MailboxEntry : AuditableEntity
{
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public MailboxType MailboxType { get; set; }

    public bool IsRead { get; set; }
    public bool IsStarred { get; set; }
    public bool IsImportant { get; set; }
    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public DateTime? SpamMarkedAt { get; set; }
    public DateTime ReceivedAt { get; set; }

    public Message Message { get; set; } = default!;
    public User User { get; set; } = default!;
}