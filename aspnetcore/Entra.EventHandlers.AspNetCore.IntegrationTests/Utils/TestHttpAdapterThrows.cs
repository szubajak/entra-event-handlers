using System.Text.Json;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Errors;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestHttpAdapterThrows : IHttpAdapter
{
    public Task<TEvent> ReadEventAsync<TEvent>(HttpContext context)
        where TEvent : EntraEvent => throw new InvalidOperationException("Read failed");

    public Task<EntraEvent> ReadEventAsync(HttpContext context) => throw new InvalidOperationException("Read failed");

    public Task WriteOkAsync(HttpContext context, EntraEventResponse response) => throw new InvalidOperationException("Write failed");

    public async Task WriteErrorAsync(HttpContext context, int statusCode, EntraErrorResponse error)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await JsonSerializer.SerializeAsync(context.Response.Body, error);
        await context.Response.Body.FlushAsync();
    }
}
