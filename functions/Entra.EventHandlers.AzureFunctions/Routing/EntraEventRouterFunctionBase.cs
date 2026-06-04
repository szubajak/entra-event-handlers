using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.Routing;

/// <summary>
/// Base class for a generic Azure Function that routes Microsoft Entra
/// custom extension events to the appropriate strongly‑typed handler.
/// </summary>
/// <remarks>
/// This router performs polymorphic deserialization of <see cref="EntraEvent"/>
/// and resolves the matching <see cref="IEntraEventHandler"/> implementation
/// based on the runtime event type. Consumers should inherit from this class
/// and expose a single HTTP‑triggered function that delegates to <see cref="Run"/>.
/// </remarks>
public abstract class EntraEventRouterFunctionBase(IServiceProvider services)
{
    private readonly IServiceProvider _services = services;

    /// <summary>
    /// Executes the routing pipeline: deserializes the incoming event,
    /// resolves the correct handler, invokes it, and returns the response
    /// as an HTTP result.
    /// </summary>
    protected async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await HttpRequestAdapter.ReadEvent(req);
        var eventType = evt.GetType();

        var handler = ResolveHandler(eventType);
        if (handler is null)
        {
            return await HttpResponseAdapter.BadRequest(
                req,
                $"No handler registered for event type '{evt.GetType().Name}'."
            );
        }

        var response = await ((dynamic)handler).Handle((dynamic)evt, context.CancellationToken);

        return await HttpResponseAdapter.From(req, response);
    }

    /// <summary>
    /// Attempts to locate a registered handler whose generic request type
    /// matches the runtime type of the incoming event.
    /// </summary>
    private IEntraEventHandler? ResolveHandler(Type eventType) =>
        _services.GetServices<IEntraEventHandler>()
            .FirstOrDefault(h =>
                h.GetType()
                 .GetInterfaces()
                 .Any(i =>
                     i.IsGenericType &&
                     i.GetGenericTypeDefinition() == typeof(IEntraEventHandler<,>) &&
                     i.GetGenericArguments()[0] == eventType));
}
