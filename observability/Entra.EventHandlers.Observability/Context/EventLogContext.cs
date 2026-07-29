using Entra.EventHandlers.Observability.Models;

namespace Entra.EventHandlers.Observability.Context;

public sealed class EventLogContext
{
    public required EventLogEntry DefaultLog { get; set; }

    public List<CustomLogEntry> CustomLogEntries { get; } = [];
}
