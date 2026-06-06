using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Entra.EventHandlers.AzureFunctions.Abstractions;

public abstract class EntraFunctionBase(IRequestAdapter requestAdapter, IResponseAdapter responseAdapter)
{
    protected IRequestAdapter RequestAdapter { get; } = requestAdapter;
    protected IResponseAdapter ResponseAdapter { get; } = responseAdapter;

    protected abstract Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context);
}
