using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSample.Functions;

public sealed class VerifiedIdClaimValidationFunction(
    ILogger<TokenIssuanceStartFunction> logger,
    IVerifiedIdClaimValidationHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : VerifiedIdClaimValidationFunctionBase(logger, handler, requestAdapter, responseAdapter)
{
    [Function("VerifiedIdClaimValidation")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "verifiedidclaimvalidation")]
        HttpRequestData req) =>
        InvokeAsync(req);
}