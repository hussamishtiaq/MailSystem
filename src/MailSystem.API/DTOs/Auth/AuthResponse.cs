namespace MailSystem.API.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public int ExpiresIn { get; set; }
    public UserInfo User { get; set; } = default!;
}

public class UserInfo
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
}