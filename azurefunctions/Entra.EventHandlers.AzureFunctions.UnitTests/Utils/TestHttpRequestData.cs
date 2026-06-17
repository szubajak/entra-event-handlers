using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Security.Claims;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Utils;

public sealed class TestHttpRequestData(FunctionContext context, Stream body)
    : HttpRequestData(context)
{
    public override Stream Body { get; } = body;

    public override HttpHeadersCollection Headers { get; } = [];

    public override IReadOnlyCollection<IHttpCookie> Cookies { get; } = [];

    public override Uri Url { get; } = new("https://localhost");

    public override IEnumerable<ClaimsIdentity> Identities { get; } = [];

    public override string Method { get; } = "POST";

    public HttpResponseData CreateResponse(HttpStatusCode statusCode)
    {
        var res = new TestHttpResponseData(FunctionContext)
        {
            StatusCode = statusCode
        };

        return res;
    }

    public override HttpResponseData CreateResponse() =>
        CreateResponse(HttpStatusCode.OK);
}
