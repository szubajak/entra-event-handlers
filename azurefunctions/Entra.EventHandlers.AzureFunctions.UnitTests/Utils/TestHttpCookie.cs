using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Utils;

public sealed class TestHttpCookie(
    string name,
    string value,
    string? domain = null,
    DateTimeOffset? expires = null,
    bool? httpOnly = null,
    double? maxAge = null,
    string? path = null,
    SameSite sameSite = SameSite.None,
    bool? secure = null) : IHttpCookie
{
    public string? Domain { get; } = domain;

    public DateTimeOffset? Expires { get; } = expires;

    public bool? HttpOnly { get; } = httpOnly;

    public double? MaxAge { get; } = maxAge;

    public string Name { get; } = name;

    public string? Path { get; } = path;

    public SameSite SameSite { get; } = sameSite;

    public bool? Secure { get; } = secure;

    public string Value { get; } = value;
}
