using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Abstractions;

public abstract class EntraSingleEndpointBase(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter,
    IEntraEventHandlerResolver resolver) : EntraEndpointBase(logger, requestAdapter, responseAdapter)
{
    protected IEntraEventHandlerResolver Resolver { get; } = resolver;
  
}
