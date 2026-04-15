using MailSystem.Domain.Entities;

namespace MailSystem.Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user);
}