using System.Text.Json;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Errors;

namespace Entra.EventHandlers.AspNetCore.Adapters;

/// <summary>
/// Default ASP.NET Core implementation of <see cref="IHttpAdapter"/> that
/// translates between HTTP requests and responses and the Entra event domain model.
/// </summary>
/// <remarks>
/// This implementation deserializes incoming <see cref="EntraEvent"/> payloads
/// from HTTP requests and serializes <see cref="EntraEventResponse"/> and
/// <see cref="EntraErrorResponse"/> objects to HTTP responses.
/// </remarks>
public sealed class HttpAdapter : IHttpAdapter
{
    /// <inheritdoc />
    public async Task<TEvent> ReadEventAsync<TEvent>(HttpContext context)
        where TEvent : EntraEvent
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync(context.RequestAborted);

            if (string.IsNullOrWhiteSpace(body))
                throw new EntraDeserializationException("Request body is empty.");

            return JsonSerializer.Deserialize<TEvent>(body) ?? throw new EntraDeserializationException("Unable to deserialize event.");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (EntraException)
        {
            throw;
        }
        catch (JsonException ex)
        {
            throw new EntraDeserializationException("Invalid JSON payload.", ex);
        }
        catch (Exception ex)
        {
            throw new EntraDeserializationException("Failed to deserialize event.", ex);
        }
    }

    /// <inheritdoc />
    public Task<EntraEvent> ReadEventAsync(HttpContext context) => ReadEventAsync<EntraEvent>(context);

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
