namespace Entra.EventHandlers.Observability.Models;

public sealed class EventLogEntry
{
    public required Guid TenantId { get; init; }
}
