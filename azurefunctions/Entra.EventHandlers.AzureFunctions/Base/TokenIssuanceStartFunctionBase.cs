using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class TokenIssuanceStartFunctionBase(ILogger logger, IHttpAdapter httpAdapter, ITokenIssuanceStartHandler handler)
    : EntraFunctionBase(logger, httpAdapter)
{
    private readonly ITokenIssuanceStartHandler _handler = handler;

    protected sealed override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await HttpAdapter.ReadEventAsync<TokenIssuanceStartEvent>(req);
        var response = await _handler.HandleAsync(evt, req.FunctionContext.CancellationToken);

        if (response.HasException)
        {
            await OnExceptionAsync(response.Exception!);
        }

        return await HttpAdapter.CreateResponseAsync(req, response.Response);
    }
}
