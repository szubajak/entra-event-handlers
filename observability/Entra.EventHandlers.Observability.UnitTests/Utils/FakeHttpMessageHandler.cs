using System.Net;

namespace Entra.EventHandlers.Observability.UnitTests.Utils;

public class FakeHttpMessageHandler(Exception? exceptionToThrow = null) : HttpMessageHandler
{
    public HttpMethod? Method { get; private set; }

    public string? RequestUri { get; private set; }

    public string? Body { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Method = request.Method;
        RequestUri = request.RequestUri?.PathAndQuery;
        Body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);

        if (exceptionToThrow is not null)
        {
            throw exceptionToThrow;
        }

        return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{}") };
    }
}
