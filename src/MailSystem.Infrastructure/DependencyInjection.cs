using MailSystem.Application.Abstractions.Authentication;
using MailSystem.Application.Abstractions.Clock;
using MailSystem.Application.Abstractions.CurrentUser;
using MailSystem.Infrastructure.Authentication;
using MailSystem.Infrastructure.CurrentUser;
using MailSystem.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace MailSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }
}