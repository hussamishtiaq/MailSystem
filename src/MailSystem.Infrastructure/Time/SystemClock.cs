using MailSystem.Application.Abstractions.Clock;

namespace MailSystem.Infrastructure.Time;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}