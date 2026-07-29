using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Models;

namespace Entra.EventHandlers.Observability.Logging;

public interface IEventLogWriter
{
    void Write(string message);
    void Write(object entry);
}

public sealed class EventLogWriter(EventLogContext ctx) : IEventLogWriter
{
    private readonly EventLogContext _ctx = ctx;

    public void Write(string message)
    {
        _ctx.CustomLogEntries.Add(new CustomLogEntry
        {
            Data = new { Message = message }
        });
    }

    public void Write(object entry)
    {
        _ctx.CustomLogEntries.Add(new CustomLogEntry
        {
            Data = entry
        });
    }
}
