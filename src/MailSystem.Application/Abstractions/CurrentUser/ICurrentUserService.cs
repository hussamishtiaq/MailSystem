namespace MailSystem.Application.Abstractions.CurrentUser;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}