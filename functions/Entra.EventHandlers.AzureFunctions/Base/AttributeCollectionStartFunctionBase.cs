using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class AttributeCollectionStartFunctionBase(
    IAttributeCollectionStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
{
    private readonly IAttributeCollectionStartHandler _handler = handler;
    private readonly IRequestAdapter _requestAdapter = requestAdapter;
    private readonly IResponseAdapter _responseAdapter = responseAdapter;

    protected async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await _requestAdapter.ReadEvent<AttributeCollectionStartEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await _responseAdapter.From(req, response);
    }
}
