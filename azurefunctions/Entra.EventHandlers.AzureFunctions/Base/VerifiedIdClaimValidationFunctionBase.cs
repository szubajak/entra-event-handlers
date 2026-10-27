using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class VerifiedIdClaimValidationFunctionBase(
    ILogger logger,
    IHttpAdapter httpAdapter,
    IVerifiedIdClaimValidationHandler handler
) : EntraFunctionBase(logger, httpAdapter)
{
    private readonly IVerifiedIdClaimValidationHandler _handler = handler;

    protected sealed override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await HttpAdapter.ReadEventAsync<VerifiedIdClaimValidationEvent>(req);
        var response = await _handler.HandleAsync(evt, req.FunctionContext.CancellationToken);

        if (response.HasException)
        {
            await OnExceptionAsync(response.Exception!);
        }

        return await HttpAdapter.CreateResponseAsync(req, response.Response);
    }
}
