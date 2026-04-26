namespace MailSystem.Application.Abstractions.Clock;

public interface IClock
{
    DateTime UtcNow { get; }
}