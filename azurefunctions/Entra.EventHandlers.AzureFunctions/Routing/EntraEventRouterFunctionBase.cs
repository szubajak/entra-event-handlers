using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Routing;

/// <summary>
/// Base class for a generic Azure Function that routes Microsoft Entra
/// custom extension events to the appropriate strongly‑typed handler.
/// </summary>
/// <remarks>
/// This router performs polymorphic deserialization of <see cref="EntraEvent"/>,
/// resolves the matching <see cref="IEntraEventHandler"/> implementation based
/// on the runtime event type, and converts known exceptions into standardized
/// <see cref="EntraErrorResponse"/> results. Consumers should inherit from this
/// class and expose a single HTTP‑triggered function that delegates to <see cref="ExecuteAsync"/>.
/// </remarks>
public abstract class EntraEventRouterFunctionBase(
    ILogger logger,
    IEntraEventHandlerResolver resolver,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraFunctionBase(logger, requestAdapter, responseAdapter)
{
    private readonly ILogger _logger = logger;
    private readonly IEntraEventHandlerResolver _resolver = resolver;

    /// <summary>
    /// Executes the routing pipeline: deserializes the incoming event,
    /// resolves the correct handler, invokes it, and returns a structured
    /// HTTP response. Known exceptions such as deserialization, validation,
    /// or handler‑resolution failures are converted into standardized
    /// <see cref="EntraErrorResponse"/> results.
    /// </summary>
    protected sealed override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await RequestAdapter.ReadEventAsync(req);
        var handler = _resolver.Resolve(evt.GetType());

        var response = await ((dynamic)handler).HandleAsync((dynamic)evt, req.FunctionContext.CancellationToken);
        return await ResponseAdapter.FromAsync(req, response);
    }

    protected override Task OnExceptionAsync(Exception ex, bool isEntraException)
    {
        if (isEntraException)
            Logger.LogWarning(ex, "Router: handled expected Entra exception.");
        else
            Logger.LogError(ex, "Router: unhandled exception while processing Entra event.");

        return Task.CompletedTask;
    }
}
