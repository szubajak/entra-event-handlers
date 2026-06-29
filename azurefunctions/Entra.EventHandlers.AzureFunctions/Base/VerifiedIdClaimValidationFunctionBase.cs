using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class VerifiedIdClaimValidationFunctionBase(
    ILogger logger,
    IVerifiedIdClaimValidationHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraFunctionBase(logger, requestAdapter, responseAdapter)
{
    private readonly IVerifiedIdClaimValidationHandler _handler = handler;

    protected sealed override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await RequestAdapter.ReadEventAsync<VerifiedIdClaimValidationEvent>(req);
        var response = await _handler.HandleAsync(evt, req.FunctionContext.CancellationToken);
        return await ResponseAdapter.FromAsync(req, response);
    }
}
