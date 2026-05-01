namespace MailSystem.API.DTOs.Mailboxes;

public class MailboxActionResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = default!;
}