using MailSystem.Domain.Common;
using MailSystem.Domain.Enums;

namespace MailSystem.Domain.Entities;

public class MessageRecipient : AuditableEntity
{
    public Guid MessageId { get; set; }
    public Guid RecipientUserId { get; set; }
    public RecipientType RecipientType { get; set; }

    public Message Message { get; set; } = default!;
    public User RecipientUser { get; set; } = default!;
}