using MailSystem.Domain.Common;

namespace MailSystem.Domain.Entities;

public class Message : AuditableEntity
{
    public Guid ConversationId { get; set; }
    public Guid SenderUserId { get; set; }
    public Guid? ParentMessageId { get; set; }

    public string Subject { get; set; } = default!;
    public string? BodyText { get; set; }
    public string? BodyHtml { get; set; }
    public bool IsDraft { get; set; }
    public DateTime? SentAt { get; set; }

    public Conversation Conversation { get; set; } = default!;
    public User SenderUser { get; set; } = default!;
    public Message? ParentMessage { get; set; }

    public ICollection<Message> Replies { get; set; } = new List<Message>();
    public ICollection<MessageRecipient> Recipients { get; set; } = new List<MessageRecipient>();
    public ICollection<MailboxEntry> MailboxEntries { get; set; } = new List<MailboxEntry>();
}