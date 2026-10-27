using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSample.Functions;

public sealed class EmailOtpSendFunction(ILogger<EmailOtpSendFunction> logger, IHttpAdapter httpAdapter, IEmailOtpSendHandler handler)
    : EmailOtpSendFunctionBase(logger, httpAdapter, handler)
{
    [Function("EmailOtpSend")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "emailotpsend")] HttpRequestData req
    ) => InvokeAsync(req);

    protected override Task OnExceptionAsync(Exception ex)
    {
        return base.OnExceptionAsync(ex);
    }
}
