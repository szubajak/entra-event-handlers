using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class AttributeCollectionStartFunctionBase(
    ILogger logger,
    IHttpAdapter httpAdapter,
    IAttributeCollectionStartHandler handler
) : EntraFunctionBase(logger, httpAdapter)
{
    private readonly IAttributeCollectionStartHandler _handler = handler;

    protected sealed override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await HttpAdapter.ReadEventAsync<AttributeCollectionStartEvent>(req);
        var result = await _handler.HandleAsync(evt, req.FunctionContext.CancellationToken);

        if (result.HasException)
        {
            await OnExceptionAsync(result.Exception!);
        }

        return await HttpAdapter.CreateResponseAsync(req, result.Response);
    }
}
