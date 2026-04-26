using System.Security.Cryptography;
using MailSystem.Application.Abstractions.Authentication;
using MailSystem.Domain.Entities;

namespace MailSystem.Infrastructure.Authentication;

public class RefreshTokenService : IRefreshTokenService
{
    public RefreshToken Generate(User user)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }
}