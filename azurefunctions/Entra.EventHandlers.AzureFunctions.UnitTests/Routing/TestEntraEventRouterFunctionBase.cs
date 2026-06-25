using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Routing;
using Entra.EventHandlers.Hosting.Resolvers;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Routing;

public sealed class TestEntraEventRouterFunctionBase(
    ILogger logger,
    IEntraEventHandlerResolver resolver,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraEventRouterFunctionBase(logger, resolver, requestAdapter, responseAdapter)
{
    public Task<HttpResponseData> RunAsync(HttpRequestData req) =>
        InvokeAsync(req);
}
