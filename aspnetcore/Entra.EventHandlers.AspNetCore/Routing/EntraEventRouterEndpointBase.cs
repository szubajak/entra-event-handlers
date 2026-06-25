using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Orchestrators;

namespace Entra.EventHandlers.AspNetCore.Routing;

/// <summary>
/// Base class for an ASP.NET Core endpoint that processes Microsoft Entra External ID
/// custom extension events by delegating execution to the shared
/// <see cref="IEntraEventOrchestrator"/>.
/// </summary>
/// <remarks>
/// This endpoint base class is responsible for:
/// <list type="bullet">
///   <item>
///     <description>Reading and deserializing the incoming HTTP request into an <see cref="EntraEvent"/>.</description>
///   </item>
///   <item>
///     <description>Invoking the hosting‑agnostic orchestrator, which routes the event to the correct strongly typed handler.</description>
///   </item>
///   <item>
///     <description>Writing the resulting <see cref="EntraEventResponse"/> to the HTTP response.</description>
///   </item>
/// </list>
/// Known exceptions such as deserialization, validation, or handler‑resolution failures
/// are converted into standardized <see cref="EntraErrorResponse"/> results by the
/// surrounding <see cref="EntraEndpointBase"/> pipeline.
/// Consumers should inherit from this class and map an ASP.NET Core endpoint
/// that delegates to <see cref="ExecuteAsync"/>.
/// </remarks>
public abstract class EntraEventRouterEndpointBase(
    ILogger logger,
    IEntraEventOrchestrator orchestrator,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraEndpointBase(logger, requestAdapter, responseAdapter)
{
    private readonly IEntraEventOrchestrator _orchestrator = orchestrator;

    /// <summary>
    /// Executes the event processing pipeline for ASP.NET Core:
    /// deserializes the incoming request, dispatches the event to the
    /// orchestrator, and writes the resulting response to the HTTP output.
    /// </summary>
    /// <param name="httpContext">The current ASP.NET Core HTTP context.</param>
    protected sealed override async Task ExecuteAsync(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEventAsync(httpContext);
        var response = await _orchestrator.DispatchAsync(evt, httpContext.RequestAborted);
        await ResponseAdapter.WriteOkAsync(httpContext, response);
    }

    /// <summary>
    /// Logs expected and unexpected exceptions encountered during event processing.
    /// </summary>
    protected override Task OnExceptionAsync(Exception ex, HttpContext context, bool isEntraException)
    {
        if (isEntraException)
            Logger.LogWarning(ex, "Router: handled expected Entra exception.");
        else
            Logger.LogError(ex, "Router: unhandled exception while processing Entra event.");

        return Task.CompletedTask;
    }
}
