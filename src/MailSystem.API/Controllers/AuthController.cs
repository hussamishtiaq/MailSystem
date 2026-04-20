using MailSystem.API.DTOs.Auth;
using MailSystem.Application.Abstractions.Authentication;
using MailSystem.Domain.Entities;
using MailSystem.Domain.Enums;
using MailSystem.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MailSystem.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly MailDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthController(
        MailDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenService refreshTokenService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();

        var exists = await _dbContext.Users
            .AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

        if (exists)
        {
            return BadRequest(new { message = "Email already exists." });
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            NormalizedEmail = normalizedEmail,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new { message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();

        var user = await _dbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

        if (user is null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        if (user.Status != UserStatus.Active)
        {
            return Unauthorized(new { message = "User is not active." });
        }

        user.LastLoginAt = DateTime.UtcNow;

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _refreshTokenService.Generate(user);

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = 900,
            User = new UserInfo
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            }
        };

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var token = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

        if (token is null || !token.IsActive)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }

        token.RevokedAt = DateTime.UtcNow;

        var newRefreshToken = _refreshTokenService.Generate(token.User);
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(token.User);

        _dbContext.RefreshTokens.Add(newRefreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresIn = 900,
            User = new UserInfo
            {
                Id = token.User.Id,
                FullName = token.User.FullName,
                Email = token.User.Email
            }
        });
    }
}