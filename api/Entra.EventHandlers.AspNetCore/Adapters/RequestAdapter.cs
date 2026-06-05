using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Entra.EventHandlers.AspNetCore.Adapters;

public interface IRequestAdapter
{
    Task<TEvent> ReadEvent<TEvent>(HttpContext context)
        where TEvent : EntraEvent;

    Task<EntraEvent> ReadEvent(HttpContext context);
}

public sealed class RequestAdapter : IRequestAdapter
{
    public async Task<TEvent> ReadEvent<TEvent>(HttpContext context)
        where TEvent : EntraEvent =>
        await JsonSerializer.DeserializeAsync<TEvent>(context.Request.Body)
            ?? throw new EntraDeserializationException("Unable to deserialize event.");

    public Task<EntraEvent> ReadEvent(HttpContext context)
        => ReadEvent<EntraEvent>(context);
}