namespace MailSystem.API.DTOs.Messages;

public class MessageDetailsResponse
{
    public Guid MessageId { get; set; }
    public Guid ConversationId { get; set; }
    public string Subject { get; set; } = default!;
    public string? BodyText { get; set; }
    public string? BodyHtml { get; set; }
    public SenderResponse Sender { get; set; } = default!;
    public List<RecipientResponse> ToRecipients { get; set; } = new();
    public List<RecipientResponse> CcRecipients { get; set; } = new();
    public List<RecipientResponse> BccRecipients { get; set; } = new();
    public DateTime? SentAt { get; set; }
    public bool IsDraft { get; set; }
}

public class SenderResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class RecipientResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string RecipientType { get; set; } = default!;
}