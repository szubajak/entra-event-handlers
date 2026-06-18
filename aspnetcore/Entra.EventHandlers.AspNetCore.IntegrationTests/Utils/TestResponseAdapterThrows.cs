using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Adapters;
using System.Text.Json;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestResponseAdapterThrows : IResponseAdapter
{
    public Task WriteOk(HttpContext context, EntraEventResponse response) =>
        throw new InvalidOperationException("Write failed");

    public Task WriteBadRequest(HttpContext context, EntraErrorResponse error) =>
        Task.CompletedTask;

    public async Task WriteServerError(HttpContext context, EntraErrorResponse error)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await JsonSerializer.SerializeAsync(context.Response.Body, error);
        await context.Response.Body.FlushAsync();
    }
}
