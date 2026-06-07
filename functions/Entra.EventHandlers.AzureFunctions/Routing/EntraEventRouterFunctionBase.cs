using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;
using Microsoft.Azure.Functions.Worker;
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
/// class and expose a single HTTP‑triggered function that delegates to <see cref="Run"/>.
/// </remarks>
public abstract class EntraEventRouterFunctionBase(
    ILogger<EntraEventRouterFunctionBase> logger,
    IEntraEventHandlerResolver resolver,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraFunctionBase(requestAdapter, responseAdapter)
{
    private readonly ILogger<EntraEventRouterFunctionBase> _logger = logger;
    private readonly IEntraEventHandlerResolver _resolver = resolver;

    /// <summary>
    /// Executes the routing pipeline: deserializes the incoming event,
    /// resolves the correct handler, invokes it, and returns a structured
    /// HTTP response. Known exceptions such as deserialization, validation,
    /// or handler‑resolution failures are converted into standardized
    /// <see cref="EntraErrorResponse"/> results.
    /// </summary>
    protected override async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        try
        {
            var evt = await RequestAdapter.ReadEvent(req);
            var handler = _resolver.Resolve(evt.GetType());

            var response = await ((dynamic)handler).Handle((dynamic)evt, context.CancellationToken);
            return await ResponseAdapter.From(req, response);
        }
        catch (Exception ex) when (ex is EntraValidationException or EntraDeserializationException or EntraHandlerNotFoundException)
        {
            _logger.LogWarning(ex, "Handled expected Entra exception.");

            var code = ex switch
            {
                EntraValidationException => EntraErrorCodes.ValidationError,
                EntraDeserializationException => EntraErrorCodes.DeserializationError,
                EntraHandlerNotFoundException => EntraErrorCodes.HandlerNotFound,
                _ => throw new InvalidOperationException("Unreachable: catch filter guarantees only known Entra exceptions.")
            };

            return await ResponseAdapter.BadRequest(
                req,
                new EntraErrorResponse
                {
                    Error = code,
                    Details = ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing Entra event.");

            return await ResponseAdapter.ServerError(
                req,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.UnhandledException,
                    Details = "An unexpected error occurred."
                });
        }
    }
}
