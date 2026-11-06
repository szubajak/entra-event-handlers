using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Abstractions;

public sealed class TestTypedEntraEndpointBase(ILogger logger, IHttpAdapter httpAdapter, IEntraEventHandlerResolver resolver)
    : EntraTypedEndpointBase<TestEvent, TestResponse>(logger, httpAdapter, resolver)
{
    protected override string Route => "test-route";

    public Task Invoke(HttpContext httpContext) => InvokeAsync(httpContext);
}
