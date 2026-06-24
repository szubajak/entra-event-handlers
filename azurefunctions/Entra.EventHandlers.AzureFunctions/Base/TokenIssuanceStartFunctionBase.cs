using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class TokenIssuanceStartFunctionBase(
    ILogger logger,
    ITokenIssuanceStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraFunctionBase(logger, requestAdapter, responseAdapter)
{
    private readonly ITokenIssuanceStartHandler _handler = handler;

    protected override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await RequestAdapter.ReadEventAsync<TokenIssuanceStartEvent>(req);
        var response = await _handler.HandleAsync(evt, req.FunctionContext.CancellationToken);
        return await ResponseAdapter.FromAsync(req, response);
    }
}
