using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Utils;

public sealed class TestHttpResponseData(FunctionContext context)
    : HttpResponseData(context)
{
    public override HttpStatusCode StatusCode { get; set; }

    public override HttpHeadersCollection Headers { get; set; } = [];

    public override Stream Body { get; set; } = new MemoryStream();

    public override HttpCookies Cookies { get; } = new TestHttpCookies();
}
