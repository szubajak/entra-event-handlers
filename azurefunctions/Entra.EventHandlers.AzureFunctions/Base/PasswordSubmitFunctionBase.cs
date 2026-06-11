using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Base;

public abstract class PasswordSubmitFunctionBase(
    ILogger logger,
    IPasswordSubmitHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraFunctionBase(logger, requestAdapter, responseAdapter)
{
    private readonly IPasswordSubmitHandler _handler = handler;

    protected override async Task<HttpResponseData> ExecuteAsync(HttpRequestData req, FunctionContext context)
    {
        var evt = await RequestAdapter.ReadEvent<PasswordSubmitEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await ResponseAdapter.From(req, response);
    }
}
