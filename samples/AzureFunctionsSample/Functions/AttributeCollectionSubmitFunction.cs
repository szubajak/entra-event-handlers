using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSample.Functions;

public sealed class AttributeCollectionSubmitFunction(
    ILogger<AttributeCollectionSubmitFunction> logger,
    IHttpAdapter httpAdapter,
    IAttributeCollectionSubmitHandler handler
) : AttributeCollectionSubmitFunctionBase(logger, httpAdapter, handler)
{
    [Function("AttributeCollectionSubmit")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "attributecollectionsubmit")] HttpRequestData req
    ) => InvokeAsync(req);
}
