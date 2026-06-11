using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSample.Functions;

public sealed class EmailOtpSendFunction(
    ILogger<EmailOtpSendFunction> logger,
    IEmailOtpSendHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EmailOtpSendFunctionBase(logger, handler, requestAdapter, responseAdapter)
{
    [Function("EmailOtpSend")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "emailotpsend")]
        HttpRequestData req,
        FunctionContext context) =>
        InvokeAsync(req, context);
}