using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSample.Functions;

public sealed class TokenIssuanceStartFunction(
    ILogger<TokenIssuanceStartFunction> logger,
    ITokenIssuanceStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : TokenIssuanceStartFunctionBase(logger, handler, requestAdapter, responseAdapter)
{
    [Function("TokenIssuanceStart")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tokenissuancestart")]
        HttpRequestData req,
        FunctionContext context)
        => Invoke(req, context);
}