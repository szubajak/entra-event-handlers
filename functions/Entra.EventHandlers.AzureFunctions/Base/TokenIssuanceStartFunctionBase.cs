using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class TokenIssuanceStartFunctionBase(
    ITokenIssuanceStartHandler handler,
    IAzureFunctionsRequestAdapter requestAdapter,
    IAzureFunctionsResponseAdapter responseAdapter)
{
    private readonly ITokenIssuanceStartHandler _handler = handler;
    private readonly IAzureFunctionsRequestAdapter _requestAdapter = requestAdapter;
    private readonly IAzureFunctionsResponseAdapter _responseAdapter = responseAdapter;

    protected async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await _requestAdapter.ReadEvent<TokenIssuanceStartEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await _responseAdapter.From(req, response);
    }
}
