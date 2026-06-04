using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Responses;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace Entra.EventHandlers.AzureFunctions.Adapters;

public static class HttpResponseAdapter
{
    public static async Task<HttpResponseData> From(HttpRequestData req, EntraEventResponse response)
    {
        var http = req.CreateResponse(HttpStatusCode.OK);
        http.Headers.Add("Content-Type", "application/json");
        await JsonSerializer.SerializeAsync(http.Body, response);
        return http;
    }

    public static Task<HttpResponseData> BadRequest(HttpRequestData req, EntraErrorResponse error) =>
        WriteError(req, HttpStatusCode.BadRequest, error);

    public static Task<HttpResponseData> ServerError(HttpRequestData req, EntraErrorResponse error) =>
        WriteError(req, HttpStatusCode.InternalServerError, error);

    private static async Task<HttpResponseData> WriteError(HttpRequestData req, HttpStatusCode status, EntraErrorResponse error)
    {
        var http = req.CreateResponse(status);
        http.Headers.Add("Content-Type", "application/json");
        await JsonSerializer.SerializeAsync(http.Body, error);
        return http;
    }
}
