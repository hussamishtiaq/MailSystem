using MailSystem.Domain.Entities;

namespace MailSystem.Application.Abstractions.Authentication;

public interface IRefreshTokenService
{
    RefreshToken Generate(User user);
}