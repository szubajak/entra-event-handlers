using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class TokenIssuanceStartFunctionBase(ITokenIssuanceStartHandler handler)
{
    private readonly ITokenIssuanceStartHandler _handler = handler;

    public async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await HttpRequestAdapter.ReadEvent<TokenIssuanceStartEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await HttpResponseAdapter.From(req, response);
    }
}
