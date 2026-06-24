using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.Hosting.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Abstractions;

public abstract class EntraFunctionBase(ILogger logger, IRequestAdapter requestAdapter, IResponseAdapter responseAdapter)
{
    protected ILogger Logger { get; } = logger;
    protected IRequestAdapter RequestAdapter { get; } = requestAdapter;
    protected IResponseAdapter ResponseAdapter { get; } = responseAdapter;

    protected virtual Task OnExceptionAsync(Exception ex, bool isEntraException)
    {
        if (isEntraException)
            Logger.LogWarning(ex, "Handled expected Entra exception.");
        else
            Logger.LogError(ex, "Unhandled exception while processing Entra event.");

        return Task.CompletedTask;
    }

    public async Task<HttpResponseData> InvokeAsync(HttpRequestData req)
    {
        try
        {
            return await ExecuteAsync(req);
        }
        catch (Exception ex) when (ex.IsEntraException())
        {
            await OnExceptionAsync(ex, isEntraException: true);

            return await ResponseAdapter.BadRequestAsync(
                req,
                new EntraErrorResponse
                {
                    Error = ex.ToEntraErrorCode(),
                    Details = ex.Message
                });
        }
        catch (Exception ex)
        {
            await OnExceptionAsync(ex, isEntraException: false);

            return await ResponseAdapter.ServerErrorAsync(
                req,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.UnhandledException,
                    Details = "An unexpected error occurred."
                });
        }
    }

    protected abstract Task<HttpResponseData> ExecuteAsync(HttpRequestData req);
}
