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
    ILogger<EntraEventRouterEndpointBase> logger,
    IEntraEventHandlerResolver resolver,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraEndpointBase(logger, requestAdapter, responseAdapter)
{
    private readonly ILogger<EntraEventRouterEndpointBase> _logger = logger;
    private readonly IEntraEventHandlerResolver _resolver = resolver;

    /// <summary>
    /// Executes the routing pipeline: deserializes the incoming event,
    /// resolves the correct handler, invokes it, and writes a structured
    /// HTTP response. Known exceptions such as deserialization, validation,
    /// or handler‑resolution failures are converted into standardized
    /// <see cref="EntraErrorResponse"/> results.
    /// </summary>
    protected override async Task Execute(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEvent(httpContext);
        var handler = _resolver.Resolve(evt.GetType());

        var response = await ((dynamic)handler).Handle((dynamic)evt, httpContext.RequestAborted);
        await ResponseAdapter.WriteOk(httpContext, response);
    }

    protected override void OnKnownException(Exception ex, HttpContext context)
    {
        _logger.LogWarning(ex, "Router: handled expected Entra exception.");
    }

    protected override void OnUnhandledException(Exception ex, HttpContext context)
    {
        _logger.LogError(ex, "Router: unhandled exception while processing Entra event.");
    }

}
