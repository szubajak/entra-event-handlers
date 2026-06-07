using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AspNetCore.Adapters;

namespace Entra.EventHandlers.AspNetCore.Abstractions;

public abstract class EntraEndpointBase(ILogger logger, IRequestAdapter requestAdapter, IResponseAdapter responseAdapter)
{
    protected ILogger Logger { get; } = logger;
    protected IRequestAdapter RequestAdapter { get; } = requestAdapter;
    protected IResponseAdapter ResponseAdapter { get; } = responseAdapter;

    protected virtual void OnKnownException(Exception ex, HttpContext context)
    {
        Logger.LogWarning(ex, "Handled expected Entra exception.");
    }

    protected virtual void OnUnhandledException(Exception ex, HttpContext context)
    {
        Logger.LogError(ex, "Unhandled exception while processing Entra event.");
    }


    protected async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await Execute(httpContext);
        }
        catch (Exception ex) when (ex is EntraValidationException or EntraDeserializationException or EntraHandlerNotFoundException)
        {
            OnKnownException(ex, httpContext);

            await ResponseAdapter.WriteBadRequest(
                httpContext,
                new EntraErrorResponse
                {
                    Error = ex switch
                    {
                        EntraValidationException => EntraErrorCodes.ValidationError,
                        EntraDeserializationException => EntraErrorCodes.DeserializationError,
                        EntraHandlerNotFoundException => EntraErrorCodes.HandlerNotFound,
                        _ => EntraErrorCodes.ValidationError
                    },
                    Details = ex.Message
                });
        }
        catch (Exception ex)
        {
            OnUnhandledException(ex, httpContext);

            await ResponseAdapter.WriteServerError(
                httpContext,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.UnhandledException,
                    Details = "An unexpected error occurred."
                });
        }
    }

    protected abstract Task Execute(HttpContext httpContext);

    public abstract void Map(IEndpointRouteBuilder endpoints);
}
