namespace Entra.EventHandlers.Observability.Dtos;

public sealed class EventLogDto
{
    public required EventLogEntryDto DefaultLog { get; init; }

    public required List<CustomLogEntryDto> CustomLogs { get; init; }
}
