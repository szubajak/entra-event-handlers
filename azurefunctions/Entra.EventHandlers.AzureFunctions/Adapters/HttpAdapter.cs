using System.Net;
using System.Text.Json;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Errors;
using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.Adapters;

/// <summary>
/// Default Azure Functions implementation of <see cref="IHttpAdapter"/>
/// that deserializes incoming Entra event payloads from an
/// <see cref="HttpRequestData"/> instance and creates
/// <see cref="HttpResponseData"/> instances containing serialized
/// Entra event responses or error payloads.
/// </summary>
public sealed class HttpAdapter : IHttpAdapter
{
    /// <inheritdoc />
    public async Task<TEvent> ReadEventAsync<TEvent>(HttpRequestData req)
        where TEvent : EntraEvent
    {
        try
        {
            using var reader = new StreamReader(req.Body);
            var body = await reader.ReadToEndAsync(req.FunctionContext.CancellationToken);

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
    public Task<EntraEvent> ReadEventAsync(HttpRequestData req) => ReadEventAsync<EntraEvent>(req);

    /// <inheritdoc />
    public async Task<HttpResponseData> CreateResponseAsync(HttpRequestData req, EntraEventResponse response)
    {
        var http = req.CreateResponse(HttpStatusCode.OK);
        http.Headers.Add("Content-Type", "application/json");
        await JsonSerializer.SerializeAsync(
            http.Body,
            response,
            response.GetType(),
            cancellationToken: req.FunctionContext.CancellationToken
        );
        return http;
    }

    /// <inheritdoc />
    public async Task<HttpResponseData> CreateErrorResponseAsync(HttpRequestData req, HttpStatusCode status, EntraErrorResponse error)
    {
        var http = req.CreateResponse(status);
        http.Headers.Add("Content-Type", "application/json");
        await JsonSerializer.SerializeAsync(http.Body, error, cancellationToken: req.FunctionContext.CancellationToken);
        return http;
    }
}
