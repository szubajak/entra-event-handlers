using System.Net;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Errors;
using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.Adapters;

/// <summary>
/// Provides an abstraction for reading Entra event payloads from an
/// <see cref="HttpRequestData"/> instance and writing Entra responses
/// to an <see cref="HttpResponseData"/> instance.
///
/// This adapter represents the HTTP boundary for Azure Functions,
/// handling serialization and deserialization of Entra Event Handlers
/// request and response payloads.
/// </summary>
public interface IHttpAdapter
{
    /// <summary>
    /// Deserializes the incoming request body into a strongly typed
    /// Entra event.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The concrete event type expected in the request body. Must derive
    /// from <see cref="EntraEvent"/>.
    /// </typeparam>
    /// <param name="req">
    /// The HTTP request containing the serialized event.
    /// </param>
    /// <returns>
    /// A task that resolves to the deserialized
    /// <typeparamref name="TEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into the
    /// expected event type.
    /// </exception>
    Task<TEvent> ReadEventAsync<TEvent>(HttpRequestData req)
        where TEvent : EntraEvent;

    /// <summary>
    /// Deserializes the incoming request body into a generic
    /// <see cref="EntraEvent"/>.
    /// Used when the concrete event type is not known at compile time.
    /// </summary>
    /// <param name="req">
    /// The HTTP request containing the serialized event.
    /// </param>
    /// <returns>
    /// A task that resolves to the deserialized
    /// <see cref="EntraEvent"/>.
    /// </returns>
    /// <exception cref="EntraDeserializationException">
    /// Thrown when the request body cannot be deserialized into an
    /// <see cref="EntraEvent"/>.
    /// </exception>
    Task<EntraEvent> ReadEventAsync(HttpRequestData req);

    /// <summary>
    /// Creates an HTTP 200 OK response containing the serialized
    /// <see cref="EntraEventResponse"/> payload.
    /// </summary>
    /// <param name="req">
    /// The incoming HTTP request used to create the response instance.
    /// </param>
    /// <param name="response">
    /// The successful Entra event response to serialize into the HTTP body.
    /// </param>
    /// <returns>
    /// A task that resolves to an <see cref="HttpResponseData"/>
    /// containing the serialized event response.
    /// </returns>
    Task<HttpResponseData> CreateResponseAsync(HttpRequestData req, EntraEventResponse response);

    /// <summary>
    /// Creates an HTTP error response containing a serialized
    /// <see cref="EntraErrorResponse"/> payload.
    /// Used when deserialization, validation, routing, or handler
    /// execution failures occur.
    /// </summary>
    /// <param name="req">
    /// The incoming HTTP request used to create the response instance.
    /// </param>
    /// <param name="statusCode">
    /// The HTTP status code to return.
    /// </param>
    /// <param name="error">
    /// The structured error describing the failure.
    /// </param>
    /// <returns>
    /// A task that resolves to an <see cref="HttpResponseData"/>
    /// containing the serialized error response.
    /// </returns>
    Task<HttpResponseData> CreateErrorResponseAsync(HttpRequestData req, HttpStatusCode statusCode, EntraErrorResponse error);
}
