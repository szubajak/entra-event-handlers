using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Adapters;
namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestResponseAdapter : IResponseAdapter
{
    public static Task WriteOk<TResponse>(HttpContext context, TResponse _)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        return Task.CompletedTask;
    }

    public Task WriteOk(HttpContext context, EntraEventResponse response)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        return Task.CompletedTask;
    }

    public Task WriteBadRequest(HttpContext context, EntraErrorResponse error)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return Task.CompletedTask;
    }

    public Task WriteServerError(HttpContext context, EntraErrorResponse error)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return Task.CompletedTask;
    }
}
