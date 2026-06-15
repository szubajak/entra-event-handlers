using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Abstractions;

public sealed class TestEntraEndpointBase(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) 
    : EntraEndpointBase(logger, requestAdapter, responseAdapter)
{
    public Func<HttpContext, Task>? ExecuteDelegate { get; set; }

    protected override Task ExecuteAsync(HttpContext httpContext)
    {
        if (ExecuteDelegate is null)
            throw new InvalidOperationException("ExecuteDelegate must be set in tests.");

        return ExecuteDelegate(httpContext);
    }

    public Task Invoke(HttpContext httpContext) => InvokeAsync(httpContext);

    public override void Map(IEndpointRouteBuilder endpoints) =>
        throw new NotSupportedException("Not needed for tests.");
}
