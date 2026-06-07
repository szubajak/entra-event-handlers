using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class AttributeCollectionStartFunctionBase(
    IAttributeCollectionStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraFunctionBase(requestAdapter, responseAdapter)
{
    private readonly IAttributeCollectionStartHandler _handler = handler;

    protected override async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await RequestAdapter.ReadEvent<AttributeCollectionStartEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await ResponseAdapter.From(req, response);
    }
}
