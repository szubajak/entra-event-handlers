using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class EmailOtpSendFunctionBase(ILogger logger, IHttpAdapter httpAdapter, IEmailOtpSendHandler handler)
    : EntraFunctionBase(logger, httpAdapter)
{
    private readonly IEmailOtpSendHandler _handler = handler;

    protected sealed override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        var evt = await HttpAdapter.ReadEventAsync<EmailOtpSendEvent>(req);
        var result = await _handler.HandleAsync(evt, req.FunctionContext.CancellationToken);

        if (result.HasException)
        {
            await OnExceptionAsync(result.Exception!);
        }

        return await HttpAdapter.CreateResponseAsync(req, result.Response);
    }
}
