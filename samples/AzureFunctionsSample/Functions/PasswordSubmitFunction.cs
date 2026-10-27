using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSample.Functions;

public sealed class PasswordSubmitFunction(ILogger<PasswordSubmitFunction> logger, IHttpAdapter httpAdapter, IPasswordSubmitHandler handler)
    : PasswordSubmitFunctionBase(logger, httpAdapter, handler)
{
    [Function("PasswordSubmit")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "passwordsubmit")] HttpRequestData req
    ) => InvokeAsync(req);
}
