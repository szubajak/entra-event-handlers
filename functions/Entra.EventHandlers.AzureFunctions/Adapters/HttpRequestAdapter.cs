using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;

namespace Entra.EventHandlers.AzureFunctions.Adapters;

/// <summary>
/// Provides an abstraction for reading and deserializing incoming Entra events
/// from an <see cref="HttpRequestData"/> instance.
/// </summary>
public interface IHttpRequestAdapter
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
    /// Thrown when the request body cannot be deserialized into the expected
    /// event type.
    /// </exception>
    Task<TEvent> ReadEvent<TEvent>(HttpRequestData req)
        where TEvent : EntraEvent;

    /// <summary>
    /// Deserializes the incoming request body into a generic <see cref="EntraEvent"/>.
    /// Used by the event router when the concrete event type is not known at
    /// compile time.
    /// </summary>
    /// <param name="req">The HTTP request containing the serialized event.</param>
    /// <returns>
    /// A task that resolves to the deserialized <see cref="EntraEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into an
    /// <see cref="EntraEvent"/>.
    /// </exception>
    Task<EntraEvent> ReadEvent(HttpRequestData req);
}

public sealed class HttpRequestAdapter : IHttpRequestAdapter
{
    /// <inheritdoc />
    public async Task<TEvent> ReadEvent<TEvent>(HttpRequestData req)
        where TEvent : EntraEvent =>
        await JsonSerializer.DeserializeAsync<TEvent>(req.Body)
            ?? throw new EntraDeserializationException("Unable to deserialize event.");

    /// <inheritdoc />
    public Task<EntraEvent> ReadEvent(HttpRequestData req) =>
        ReadEvent<EntraEvent>(req);
}
