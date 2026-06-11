using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Base;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class AttributeCollectionStartEndpoint(
    ILogger<AttributeCollectionStartEndpoint> logger,
    IAttributeCollectionStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : AttributeCollectionStartEndpointBase(logger, handler, requestAdapter, responseAdapter)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("attributecollectionstart", InvokeAsync);
    }
}