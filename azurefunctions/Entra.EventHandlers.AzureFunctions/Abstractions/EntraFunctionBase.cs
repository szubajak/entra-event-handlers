using System.Net;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Extensions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.Hosting.Errors;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Abstractions;

public abstract class EntraFunctionBase(ILogger logger, IHttpAdapter httpAdapter)
{
    protected ILogger Logger { get; } = logger;
    protected IHttpAdapter HttpAdapter { get; } = httpAdapter;

    protected virtual Task OnExceptionAsync(Exception ex) => Task.CompletedTask;

    public async Task<HttpResponseData> InvokeAsync(HttpRequestData req)
    {
        try
        {
            return await ExecuteAsync(req);
        }
        catch (OperationCanceledException ex)
        {
            Logger.LogInformation(ex, "Entra event handling was canceled in hosting layer.");

            await OnExceptionAsync(ex);

            return await HttpAdapter.CreateErrorResponseAsync(
                req,
                HttpStatusCode.RequestTimeout,
                new EntraErrorResponse { Error = EntraErrorCodes.RequestCanceled, Details = "The request was canceled." }
            );
        }
        catch (Exception ex) when (ex.IsEntraException())
        {
            Logger.LogWarning(ex, "Entra domain exception occurred in hosting layer during Entra event handling.");

            await OnExceptionAsync(ex);

            return await HttpAdapter.CreateErrorResponseAsync(
                req,
                HttpStatusCode.BadRequest,
                new EntraErrorResponse { Error = ex.ToEntraErrorCode(), Details = ex.Message }
            );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected failure occurred in hosting layer during Entra event handling.");

            await OnExceptionAsync(ex);

            return await HttpAdapter.CreateErrorResponseAsync(
                req,
                HttpStatusCode.InternalServerError,
                new EntraErrorResponse { Error = EntraErrorCodes.UnhandledException, Details = "Unexpected failure occurred." }
            );
        }
    }

    protected abstract Task<HttpResponseData> ExecuteAsync(HttpRequestData req);
}
