using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;

namespace Entra.EventHandlers.AzureFunctions.Adapters;

/// <summary>
/// Provides an abstraction for reading and deserializing incoming Entra events
/// from an <see cref="HttpRequestData"/> instance.
/// </summary>
public interface IRequestAdapter
{
    /// <summary>
    /// Deserializes the incoming request body into a strongly typed Entra event.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The concrete event type expected in the request body. Must derive from
    /// <see cref="EntraEvent"/>.
    /// </typeparam>
    /// <param name="req">The HTTP request containing the serialized event.</param>
    /// <returns>
    /// A task that resolves to the deserialized <typeparamref name="TEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into the expected event type.
    /// </exception>
    Task<TEvent> ReadEventAsync<TEvent>(HttpRequestData req)
        where TEvent : EntraEvent;

    /// <summary>
    /// Deserializes the incoming request body into a generic <see cref="EntraEvent"/>.
    /// Used by the event router when the concrete event type is not known at compile time.
    /// </summary>
    /// <param name="req">The HTTP request containing the serialized event.</param>
    /// <returns>
    /// A task that resolves to the deserialized <see cref="EntraEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into an <see cref="EntraEvent"/>.
    /// </exception>
    Task<EntraEvent> ReadEventAsync(HttpRequestData req);
}

/// <summary>
/// Default Azure Functions implementation of <see cref="IRequestAdapter"/> that
/// deserializes incoming Entra event payloads from an <see cref="HttpRequestData"/>
/// instance.
/// </summary>
public sealed class RequestAdapter : IRequestAdapter
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

            return JsonSerializer.Deserialize<TEvent>(body)
                ?? throw new EntraDeserializationException("Unable to deserialize event.");
        }
        catch (Exception ex)
        {
            throw ex switch
            {
                JsonException jex => new EntraDeserializationException("Invalid JSON payload.", jex),
                EntraDeserializationException => ex,
                _ => new EntraDeserializationException("Failed to deserialize event.", ex)
            };
        }
    }

    /// <inheritdoc />
    public Task<EntraEvent> ReadEventAsync(HttpRequestData req) =>
        ReadEventAsync<EntraEvent>(req);
}
