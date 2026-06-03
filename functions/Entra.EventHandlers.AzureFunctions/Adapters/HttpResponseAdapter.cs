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
}
