using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Extensions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Interfaces;
using Entra.EventHandlers.Hosting.Errors;

namespace Entra.EventHandlers.AspNetCore.Abstractions;

public abstract class EntraEndpointBase(ILogger logger, IRequestAdapter requestAdapter, IResponseAdapter responseAdapter)
{
    protected ILogger Logger { get; } = logger;
    protected IRequestAdapter RequestAdapter { get; } = requestAdapter;
    protected IResponseAdapter ResponseAdapter { get; } = responseAdapter;

    protected Task OnExceptionAsync(Exception ex, HttpContext context)
    {
        var exceptionHandler = context.RequestServices.GetService<IEntraExceptionHandler>();
        if (exceptionHandler is not null)
            return exceptionHandler.HandleAsync(ex);

        return Task.CompletedTask;
    }

    protected async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await ExecuteAsync(httpContext);
        }
        catch (OperationCanceledException ex)
        {
            Logger.LogInformation(ex, "Entra event handling was canceled in hosting layer.");

            await OnExceptionAsync(ex, httpContext);

            await ResponseAdapter.WriteErrorAsync(
                httpContext,
                StatusCodes.Status408RequestTimeout,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.RequestCanceled,
                    Details = "The request was canceled."
                });
        }
        catch (Exception ex) when (ex.IsEntraException())
        {
            Logger.LogWarning(ex, "Entra domain exception occurred in hosting layer during Entra event handling.");

            await OnExceptionAsync(ex, httpContext);

            await ResponseAdapter.WriteErrorAsync(
                httpContext,
                StatusCodes.Status400BadRequest,
                new EntraErrorResponse
                {
                    Error = ex.ToEntraErrorCode(),
                    Details = ex.Message
                });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected failure occurred in hosting layer during Entra event handling.");

            await OnExceptionAsync(ex, httpContext);

            await ResponseAdapter.WriteErrorAsync(
                httpContext,
                StatusCodes.Status500InternalServerError,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.UnhandledException,
                    Details = "Unexpected failure occurred."
                });
        }
    }

    protected abstract Task ExecuteAsync(HttpContext httpContext);

    public abstract void Map(IEndpointRouteBuilder endpoints);
}
