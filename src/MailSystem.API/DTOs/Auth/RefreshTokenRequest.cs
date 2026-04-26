namespace MailSystem.API.DTOs.Auth;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = default!;
}