using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.Hosting.Orchestrators;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Routing;

/// <summary>
/// Base class for an Azure Function that processes Microsoft Entra External ID
/// custom extension events by delegating execution to the shared
/// <see cref="IEntraEventOrchestrator"/>.
/// </summary>
/// <remarks>
/// This function base class is responsible for:
/// <list type="bullet">
///   <item>
///     <description>Reading and deserializing the incoming HTTP request into an <see cref="EntraEvent"/>.</description>
///   </item>
///   <item>
///     <description>Invoking the hosting‑agnostic orchestrator, which routes the event to the correct strongly typed handler.</description>
///   </item>
///   <item>
///     <description>Converting the resulting <see cref="EntraEventResponse"/> into an <see cref="HttpResponseData"/>.</description>
///   </item>
/// </list>
/// Known exceptions such as deserialization, validation, or handler‑resolution failures
/// are converted into standardized <see cref="EntraErrorResponse"/> results by the
/// surrounding <see cref="EntraFunctionBase"/> pipeline.
/// Consumers should inherit from this class and expose a single HTTP‑triggered
/// Azure Function that delegates to <see cref="ExecuteAsync"/>.
/// </remarks>
public abstract class EntraEventRouterFunctionBase(
    ILogger logger,
    IEntraEventOrchestrator orchestrator,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraFunctionBase(logger, requestAdapter, responseAdapter)
{
    private readonly IEntraEventOrchestrator _orchestrator = orchestrator;

    /// <summary>
    /// Executes the event processing pipeline for Azure Functions:
    /// deserializes the incoming request, dispatches the event to the
    /// orchestrator, and converts the resulting response into an HTTP result.
    /// </summary>
    /// <param name="req">The incoming Azure Functions HTTP request.</param>
    /// <returns>
    /// A structured <see cref="HttpResponseData"/> representing either a successful
    /// handler response or a standardized <see cref="EntraErrorResponse"/> in case
    /// of known failures.
    /// </returns>
    protected sealed override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await RequestAdapter.ReadEventAsync(req);
        var response = await _orchestrator.DispatchAsync(evt, req.FunctionContext.CancellationToken);
        return await ResponseAdapter.FromAsync(req, response);
    }

    /// <summary>
    /// Logs expected and unexpected exceptions encountered during event processing.
    /// </summary>
    protected override Task OnExceptionAsync(Exception ex, bool isEntraException)
    {
        if (isEntraException)
            Logger.LogWarning(ex, "Router: handled expected Entra exception.");
        else
            Logger.LogError(ex, "Router: unhandled exception while processing Entra event.");

        return Task.CompletedTask;
    }
}
