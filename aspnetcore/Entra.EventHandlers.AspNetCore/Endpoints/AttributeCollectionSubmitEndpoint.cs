using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class AttributeCollectionSubmitEndpoint(
    ILogger<AttributeCollectionSubmitEndpoint> logger,
    IHttpAdapter httpAdapter,
    IEntraEventHandlerResolver resolver
) : EntraTypedEndpointBase<AttributeCollectionSubmitEvent, AttributeCollectionSubmitResponse>(logger, httpAdapter, resolver)
{
    protected override string Route => "attributecollectionsubmit";
}
