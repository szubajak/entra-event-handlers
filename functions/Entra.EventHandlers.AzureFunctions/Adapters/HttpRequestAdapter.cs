using Entra.EventHandlers.Abstractions.Events;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;

namespace Entra.EventHandlers.AzureFunctions.Adapters;

public static class HttpRequestAdapter
{
    public static async Task<TEvent> ReadEvent<TEvent>(HttpRequestData req)
        where TEvent : EntraEvent =>
        await JsonSerializer.DeserializeAsync<TEvent>(req.Body)
            ?? throw new InvalidOperationException("Unable to deserialize event.");

    public static Task<EntraEvent> ReadEvent(HttpRequestData req) =>
        ReadEvent<EntraEvent>(req);
}
