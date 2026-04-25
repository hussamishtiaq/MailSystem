namespace MailSystem.API.DTOs.Messages;

public class SaveDraftRequest
{
    public string Subject { get; set; } = default!;
    public string? BodyText { get; set; }
    public string? BodyHtml { get; set; }
    public List<Guid> ToUserIds { get; set; } = new();
    public List<Guid> CcUserIds { get; set; } = new();
    public List<Guid> BccUserIds { get; set; } = new();
}