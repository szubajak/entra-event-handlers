using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Routing;

/// <summary>
/// Base class for a generic ASP.NET Core endpoint that routes Microsoft Entra
/// custom extension events to the appropriate strongly‑typed handler.
/// </summary>
/// <remarks>
/// This router performs polymorphic deserialization of <see cref="EntraEvent"/>,
/// resolves the matching <see cref="IEntraEventHandler"/> implementation based
/// on the runtime event type, and converts known exceptions into standardized
/// <see cref="EntraErrorResponse"/> results. Consumers should inherit from this
/// class and map an HTTP endpoint that delegates to <see cref="Invoke"/>.
/// </remarks>
public abstract class EntraEventRouterEndpointBase(
    ILogger logger,
    IEntraEventHandlerResolver resolver,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraEndpointBase(logger, requestAdapter, responseAdapter)
{
    private readonly ILogger _logger = logger;
    private readonly IEntraEventHandlerResolver _resolver = resolver;

    /// <summary>
    /// Executes the routing pipeline: deserializes the incoming event,
    /// resolves the correct handler, invokes it, and writes a structured
    /// HTTP response. Known exceptions such as deserialization, validation,
    /// or handler‑resolution failures are converted into standardized
    /// <see cref="EntraErrorResponse"/> results.
    /// </summary>
    protected override async Task ExecuteAsync(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEvent(httpContext);
        var handler = _resolver.Resolve(evt.GetType());

        var response = await ((dynamic)handler).Handle((dynamic)evt, httpContext.RequestAborted);
        await ResponseAdapter.WriteOk(httpContext, response);
    }

    protected override Task OnExceptionAsync(Exception ex, HttpContext context, bool isEntraException)
    {
        if (isEntraException)
            Logger.LogWarning(ex, "Router: handled expected Entra exception.");
        else
            Logger.LogError(ex, "Router: unhandled exception while processing Entra event.");

        return Task.CompletedTask;
    }
}
