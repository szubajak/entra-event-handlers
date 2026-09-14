using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class AttributeCollectionStartFunctionBase(
    ILogger logger,
    IAttributeCollectionStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraFunctionBase(logger, requestAdapter, responseAdapter)
{
    private readonly IAttributeCollectionStartHandler _handler = handler;

    protected sealed override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await RequestAdapter.ReadEventAsync<AttributeCollectionStartEvent>(req);
        var result = await _handler.HandleAsync(evt, req.FunctionContext.CancellationToken);

        if (result.HasException)
        {
            await OnExceptionAsync(result.Exception!);
        }

        return await ResponseAdapter.FromAsync(req, result.Response);
    }
}
