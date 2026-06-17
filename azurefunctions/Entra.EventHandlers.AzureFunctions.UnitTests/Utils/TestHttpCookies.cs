using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Utils;

public sealed class TestHttpCookies : HttpCookies
{
    private readonly Dictionary<string, IHttpCookie> _cookies = [];

    public override void Append(string name, string value)
    {
        _cookies[name] = new TestHttpCookie(name, value);
    }

    public override void Append(IHttpCookie cookie)
    {
        _cookies[cookie.Name] = cookie;
    }

    public override IHttpCookie CreateNew()
    {
        return new TestHttpCookie(string.Empty, string.Empty);
    }

    public IReadOnlyDictionary<string, IHttpCookie> Items => _cookies;
}
