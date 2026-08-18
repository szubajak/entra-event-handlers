namespace Entra.EventHandlers.Observability.Dtos;

public sealed class CustomLogEntryDto
{
    public DateTime Timestamp { get; init; }

    public required object Data { get; init; }
}
