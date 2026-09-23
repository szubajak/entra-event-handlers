using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Errors;
using System.Text.Json;

namespace Entra.EventHandlers.AspNetCore.Adapters;

/// <summary>
/// Provides an abstraction for writing <see cref="EntraEventResponse"/> and
/// <see cref="EntraErrorResponse"/> objects into an <see cref="HttpResponse"/>
/// associated with an <see cref="HttpContext"/>.
/// </summary>
public interface IResponseAdapter
{
    /// <summary>
    /// Writes an HTTP 200 OK response containing the serialized
    /// <see cref="EntraEventResponse"/> payload.
    /// </summary>
    /// <param name="context">
    /// The HTTP context whose response stream will receive the serialized payload.
    /// </param>
    /// <param name="response">
    /// The successful Entra event response to serialize into the HTTP body.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous write operation.
    /// </returns>
    Task WriteOkAsync(HttpContext context, EntraEventResponse response);

    /// <summary>
    /// Writes an HTTP Error response containing a serialized
    /// <see cref="EntraErrorResponse"/> payload. Used when deserialization,
    /// validation, or handler‑resolution failures occur.
    /// </summary>
    /// <param name="context">
    /// The HTTP context whose response stream will receive the serialized payload.
    /// </param>
    /// <param name="statusCode">
    /// The HTTP status code.
    /// </param>
    /// <param name="error">
    /// The structured error describing the failure.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous write operation.
    /// </returns>
    Task WriteErrorAsync(HttpContext context, int statusCode, EntraErrorResponse error);
}

/// <summary>
/// Default ASP.NET Core implementation of <see cref="IResponseAdapter"/> that
/// serializes Entra event responses into the HTTP response stream.
/// </summary>
public class ResponseAdapter : IResponseAdapter
{
    /// <inheritdoc />
    public async Task WriteOkAsync(HttpContext context, EntraEventResponse response)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "application/json";
        await JsonSerializer.SerializeAsync(context.Response.Body, response, response.GetType(), cancellationToken: context.RequestAborted);
        await context.Response.Body.FlushAsync();
    }

    /// <inheritdoc />
    public async Task WriteErrorAsync(HttpContext context, int statusCode, EntraErrorResponse error)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await JsonSerializer.SerializeAsync(context.Response.Body, error, cancellationToken: context.RequestAborted);
        await context.Response.Body.FlushAsync();
    }
}
