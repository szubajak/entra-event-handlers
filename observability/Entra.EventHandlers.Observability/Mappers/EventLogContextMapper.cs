using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Dtos;
using Entra.EventHandlers.Observability.Models;

namespace Entra.EventHandlers.Observability.Mappers;

public interface IEventLogContextMapper
{
    EventLogDto Map(EventLogContext ctx);
}
public sealed class EventLogContextMapper : IEventLogContextMapper
{
    public EventLogDto Map(EventLogContext ctx) =>
        new()
        {
            DefaultLog = MapDefaultLog(ctx.DefaultLog),
            CustomLogs = MapCustomLogs(ctx.CustomLogEntries)
        };

    private static EventLogEntryDto MapDefaultLog(EventLogEntry entry) =>
        new()
        {
            TenantId = entry.TenantId
        };

    private static List<CustomLogEntryDto> MapCustomLogs(List<CustomLogEntry> entries)
    {
        var list = new List<CustomLogEntryDto>(entries.Count);

        foreach (var e in entries)
        {
            if (e is null || e.Data is null)
                continue;

            list.Add(new CustomLogEntryDto
            {
                Timestamp = e.Timestamp,
                Data = e.Data
            });
        }

        return list;
    }
}
