using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSample.Functions;

public sealed class AttributeCollectionSubmitFunction(
    ILogger<AttributeCollectionSubmitFunction> logger,
    IAttributeCollectionSubmitHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : AttributeCollectionSubmitFunctionBase(logger, handler, requestAdapter, responseAdapter)
{
    [Function("AttributeCollectionSubmit")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "attributecollectionsubmit")]
        HttpRequestData req,
        FunctionContext context)
        => Invoke(req, context);
}