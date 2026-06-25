using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSample.Functions;

public sealed class AttributeCollectionStartFunction(
    ILogger<AttributeCollectionStartFunction> logger,
    IAttributeCollectionStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : AttributeCollectionStartFunctionBase(logger, handler, requestAdapter, responseAdapter)
{
    [Function("AttributeCollectionStart")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "attributecollectionstart")]
        HttpRequestData req) => 
        InvokeAsync(req);
}