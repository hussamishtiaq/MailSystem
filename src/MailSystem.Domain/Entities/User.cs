using MailSystem.Domain.Common;
using MailSystem.Domain.Enums;

namespace MailSystem.Domain.Entities;

public class User : AuditableEntity
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string NormalizedEmail { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserStatus Status { get; set; } = UserStatus.Active;
    public bool IsEmailVerified { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<MessageRecipient> ReceivedRecipients { get; set; } = new List<MessageRecipient>();
    public ICollection<MailboxEntry> MailboxEntries { get; set; } = new List<MailboxEntry>();
    public ICollection<Conversation> CreatedConversations { get; set; } = new List<Conversation>();
}