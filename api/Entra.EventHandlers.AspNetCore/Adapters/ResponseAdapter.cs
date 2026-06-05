using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Responses;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Entra.EventHandlers.AspNetCore.Adapters;

public interface IResponseAdapter
{
    Task WriteOk(HttpContext context, EntraEventResponse response);

    Task WriteBadRequest(HttpContext context, EntraErrorResponse error);

    Task WriteServerError(HttpContext context, EntraErrorResponse error);
}


public class ResponseAdapter : IResponseAdapter
{
    public async Task WriteOk(HttpContext context, EntraEventResponse response)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "application/json";
        await JsonSerializer.SerializeAsync(context.Response.Body, response);
    }

    public Task WriteBadRequest(HttpContext context, EntraErrorResponse error) =>
        WriteError(context, StatusCodes.Status400BadRequest, error);

    public Task WriteServerError(HttpContext context, EntraErrorResponse error) =>
        WriteError(context, StatusCodes.Status500InternalServerError, error);

    private static async Task WriteError(HttpContext context, int statusCode, EntraErrorResponse error)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await JsonSerializer.SerializeAsync(context.Response.Body, error);
    }
}
