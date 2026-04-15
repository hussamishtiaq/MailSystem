using MailSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MailSystem.Persistence.Context;

public class MailDbContext : DbContext
{
    public MailDbContext(DbContextOptions<MailDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MailDbContext).Assembly);
    }
}