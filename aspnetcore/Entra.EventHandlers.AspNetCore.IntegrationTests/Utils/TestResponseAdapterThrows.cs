using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Errors;
using System.Text.Json;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestResponseAdapterThrows : IResponseAdapter
{
    public Task WriteOkAsync(HttpContext context, EntraEventResponse response) =>
        throw new InvalidOperationException("Write failed");

    public async Task WriteErrorAsync(HttpContext context, int statusCode, EntraErrorResponse error)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await JsonSerializer.SerializeAsync(context.Response.Body, error);
        await context.Response.Body.FlushAsync();
    }
}
