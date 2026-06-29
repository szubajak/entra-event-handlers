using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Extensions;

namespace Entra.EventHandlers.AspNetCore.Abstractions;

public abstract class EntraEndpointBase(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
{
    protected ILogger Logger { get; } = logger;
    protected IRequestAdapter RequestAdapter { get; } = requestAdapter;
    protected IResponseAdapter ResponseAdapter { get; } = responseAdapter;

    protected virtual Task OnExceptionAsync(Exception ex, HttpContext context, bool isEntraException)
    {
        if (isEntraException)
            Logger.LogWarning(ex, "Handled expected Entra exception.");
        else
            Logger.LogError(ex, "Unhandled exception while processing Entra event.");

        return Task.CompletedTask;
    }

    protected async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await ExecuteAsync(httpContext);
        }
        catch (Exception ex) when (ex.IsEntraException())
        {
            await OnExceptionAsync(ex, httpContext, isEntraException: true);

            await ResponseAdapter.WriteBadRequestAsync(
                httpContext,
                new EntraErrorResponse
                {
                    Error = ex.ToEntraErrorCode(),
                    Details = ex.Message
                });
        }
        catch (Exception ex)
        {
            await OnExceptionAsync(ex, httpContext, isEntraException: false);

            await ResponseAdapter.WriteServerErrorAsync(
                httpContext,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.UnhandledException,
                    Details = "An unexpected error occurred."
                });
        }
    }

    protected abstract Task ExecuteAsync(HttpContext httpContext);

    public abstract void Map(IEndpointRouteBuilder endpoints);
}
