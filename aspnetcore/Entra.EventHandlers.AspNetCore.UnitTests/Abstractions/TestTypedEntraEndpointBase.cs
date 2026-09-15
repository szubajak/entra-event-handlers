using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Abstractions;

public sealed class TestTypedEntraEndpointBase(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter,
    IEntraEventHandlerResolver resolver) 
    : EntraTypedEndpointBase<TestEvent, TestResponse>(logger, requestAdapter, responseAdapter, resolver)
{
    public Task Invoke(HttpContext httpContext) => InvokeAsync(httpContext);

    public override void Map(IEndpointRouteBuilder endpoints) =>
        throw new NotSupportedException("Not needed for tests.");
}
