using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Entra.EventHandlers.AspNetCore.Adapters;

/// <summary>
/// Provides an abstraction for reading and deserializing incoming Entra events
/// from an <see cref="HttpContext"/> instance.
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
    /// <param name="context">The HTTP context containing the serialized event.</param>
    /// <returns>
    /// A task that resolves to the deserialized <typeparamref name="TEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into the expected event type.
    /// </exception>
    Task<TEvent> ReadEvent<TEvent>(HttpContext context)
        where TEvent : EntraEvent;

    /// <summary>
    /// Deserializes the incoming request body into a generic <see cref="EntraEvent"/>.
    /// Used by the event router when the concrete event type is not known at compile time.
    /// </summary>
    /// <param name="context">The HTTP context containing the serialized event.</param>
    /// <returns>
    /// A task that resolves to the deserialized <see cref="EntraEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into an <see cref="EntraEvent"/>.
    /// </exception>
    Task<EntraEvent> ReadEvent(HttpContext context);
}

/// <summary>
/// Default ASP.NET Core implementation of <see cref="IRequestAdapter"/> that
/// deserializes incoming Entra event payloads from the request body of an
/// <see cref="HttpContext"/>.
/// </summary>
public sealed class RequestAdapter : IRequestAdapter
{
    /// <inheritdoc />
    public async Task<TEvent> ReadEvent<TEvent>(HttpContext context)
        where TEvent : EntraEvent =>
        await JsonSerializer.DeserializeAsync<TEvent>(context.Request.Body)
            ?? throw new EntraDeserializationException("Unable to deserialize event.");

    /// <inheritdoc />
    public Task<EntraEvent> ReadEvent(HttpContext context)
        => ReadEvent<EntraEvent>(context);
}