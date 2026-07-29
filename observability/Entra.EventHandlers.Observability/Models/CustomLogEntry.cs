namespace Entra.EventHandlers.Observability.Models;

public sealed class CustomLogEntry
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public required object Data { get; init; }
}
