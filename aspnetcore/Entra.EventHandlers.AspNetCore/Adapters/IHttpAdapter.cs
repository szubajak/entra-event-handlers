using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Errors;

namespace Entra.EventHandlers.AspNetCore.Adapters;

/// <summary>
/// Provides an abstraction for translating between HTTP requests and responses
/// and the Entra event domain model.
/// </summary>
/// <remarks>
/// Implementations are responsible for deserializing incoming
/// <see cref="EntraEvent"/> payloads from an <see cref="HttpContext"/> and
/// serializing <see cref="EntraEventResponse"/> and
/// <see cref="EntraErrorResponse"/> objects to the HTTP response.
/// </remarks>
public interface IHttpAdapter
{
    /// <summary>
    /// Deserializes the incoming request body into a strongly typed Entra event.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The concrete event type expected in the request body. Must derive from
    /// <see cref="EntraEvent"/>.
    /// </typeparam>
    /// <param name="context">
    /// The HTTP context containing the serialized event.
    /// </param>
    /// <returns>
    /// A task that resolves to the deserialized <typeparamref name="TEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into the expected event type.
    /// </exception>
    Task<TEvent> ReadEventAsync<TEvent>(HttpContext context)
        where TEvent : EntraEvent;

    /// <summary>
    /// Deserializes the incoming request body into a generic
    /// <see cref="EntraEvent"/>.
    /// Used when the concrete event type is not known at compile time.
    /// </summary>
    /// <param name="context">
    /// The HTTP context containing the serialized event.
    /// </param>
    /// <returns>
    /// A task that resolves to the deserialized <see cref="EntraEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into an
    /// <see cref="EntraEvent"/>.
    /// </exception>
    Task<EntraEvent> ReadEventAsync(HttpContext context);

    /// <summary>
    /// Writes an HTTP 200 OK response containing the serialized
    /// <see cref="EntraEventResponse"/> payload.
    /// </summary>
    /// <param name="context">
    /// The HTTP context whose response stream will receive the serialized payload.
    /// </param>
    /// <param name="response">
    /// The successful Entra event response to serialize into the HTTP response.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous write operation.
    /// </returns>
    Task WriteOkAsync(HttpContext context, EntraEventResponse response);

    /// <summary>
    /// Writes an HTTP error response containing a serialized
    /// <see cref="EntraErrorResponse"/> payload.
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
