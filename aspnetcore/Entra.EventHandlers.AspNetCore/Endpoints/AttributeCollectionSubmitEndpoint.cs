using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Base;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class AttributeCollectionSubmitEndpoint(
    ILogger<AttributeCollectionSubmitEndpoint> logger,
    IAttributeCollectionSubmitHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : AttributeCollectionSubmitEndpointBase(logger, handler, requestAdapter, responseAdapter)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("attributecollectionsubmit", InvokeAsync);
    }
}