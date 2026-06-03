using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class AttributeCollectionStartFunctionBase(IAttributeCollectionStartHandler handler)
{
    private readonly IAttributeCollectionStartHandler _handler = handler;

    protected async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await HttpRequestAdapter.ReadEvent<AttributeCollectionStartEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await HttpResponseAdapter.From(req, response);
    }
}
