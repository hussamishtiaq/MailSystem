using MailSystem.Domain.Common;

namespace MailSystem.Domain.Entities;

public class Conversation : AuditableEntity
{
    public string Subject { get; set; } = default!;
    public Guid CreatedByUserId { get; set; }
    public DateTime LastMessageAt { get; set; }

    public User CreatedByUser { get; set; } = default!;
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}