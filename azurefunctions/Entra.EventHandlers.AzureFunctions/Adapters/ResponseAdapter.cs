using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Responses;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace Entra.EventHandlers.AzureFunctions.Adapters;

/// <summary>
/// Provides an abstraction for writing <see cref="EntraEventResponse"/> and
/// <see cref="EntraErrorResponse"/> objects into an <see cref="HttpResponseData"/> instance.
/// </summary>
public interface IResponseAdapter
{
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
    /// A task that resolves to an <see cref="HttpResponseData"/> containing
    /// the serialized event response.
    /// </returns>
    Task<HttpResponseData> FromAsync(HttpRequestData req, EntraEventResponse response);

    /// <summary>
    /// Creates an HTTP 400 Bad Request response containing a serialized
    /// <see cref="EntraErrorResponse"/> payload. Used when deserialization,
    /// validation, or handler‑resolution failures occur.
    /// </summary>
    /// <param name="req">
    /// The incoming HTTP request used to create the response instance.
    /// </param>
    /// <param name="error">
    /// The structured error describing the failure.
    /// </param>
    /// <returns>
    /// A task that resolves to an <see cref="HttpResponseData"/> containing
    /// the serialized error response.
    /// </returns>
    Task<HttpResponseData> BadRequestAsync(HttpRequestData req, EntraErrorResponse error);

    /// <summary>
    /// Creates an HTTP 500 Internal Server Error response containing a
    /// serialized <see cref="EntraErrorResponse"/> payload. Used when an
    /// unexpected exception occurs during event processing.
    /// </summary>
    /// <param name="req">
    /// The incoming HTTP request used to create the response instance.
    /// </param>
    /// <param name="error">
    /// The structured error describing the unexpected failure.
    /// </param>
    /// <returns>
    /// A task that resolves to an <see cref="HttpResponseData"/> containing
    /// the serialized error response.
    /// </returns>
    Task<HttpResponseData> ServerErrorAsync(HttpRequestData req, EntraErrorResponse error);
}

/// <summary>
/// Default Azure Functions implementation of <see cref="IResponseAdapter"/> that
/// creates <see cref="HttpResponseData"/> instances containing serialized Entra
/// event responses or error payloads.
/// </summary>
public sealed class ResponseAdapter : IResponseAdapter
{
    /// <inheritdoc />
    public async Task<HttpResponseData> FromAsync(HttpRequestData req, EntraEventResponse response)
    {
        var http = req.CreateResponse(HttpStatusCode.OK);
        http.Headers.Add("Content-Type", "application/json");
        await JsonSerializer.SerializeAsync(http.Body, response, response.GetType(), cancellationToken: req.FunctionContext.CancellationToken);
        return http;
    }

    /// <inheritdoc />
    public Task<HttpResponseData> BadRequestAsync(HttpRequestData req, EntraErrorResponse error) =>
        WriteErrorAsync(req, HttpStatusCode.BadRequest, error);

    /// <inheritdoc />
    public Task<HttpResponseData> ServerErrorAsync(HttpRequestData req, EntraErrorResponse error) =>
        WriteErrorAsync(req, HttpStatusCode.InternalServerError, error);

    private static async Task<HttpResponseData> WriteErrorAsync(HttpRequestData req, HttpStatusCode status, EntraErrorResponse error)
    {
        var http = req.CreateResponse(status);
        http.Headers.Add("Content-Type", "application/json");
        await JsonSerializer.SerializeAsync(http.Body, error, cancellationToken: req.FunctionContext.CancellationToken);
        return http;
    }
}
