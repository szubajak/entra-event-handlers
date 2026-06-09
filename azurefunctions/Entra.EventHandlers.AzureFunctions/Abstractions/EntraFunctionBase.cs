using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.Abstractions;

public abstract class EntraFunctionBase(ILogger logger, IRequestAdapter requestAdapter, IResponseAdapter responseAdapter)
{
    protected ILogger Logger { get; } = logger;
    protected IRequestAdapter RequestAdapter { get; } = requestAdapter;
    protected IResponseAdapter ResponseAdapter { get; } = responseAdapter;

    protected virtual void OnKnownException(Exception ex, FunctionContext context)
    {
        Logger.LogWarning(ex, "Handled expected Entra exception.");
    }

    protected virtual void OnUnhandledException(Exception ex, FunctionContext context)
    {
        Logger.LogError(ex, "Unhandled exception while processing Entra event.");
    }

    public async Task<HttpResponseData> Invoke(HttpRequestData req, FunctionContext context)
    {
        try
        {
            return await Execute(req, context);
        }
        catch (Exception ex) when (ex is EntraValidationException or EntraDeserializationException or EntraHandlerNotFoundException)
        {
            OnKnownException(ex, context);

            var code = ex switch
            {
                EntraValidationException => EntraErrorCodes.ValidationError,
                EntraDeserializationException => EntraErrorCodes.DeserializationError,
                EntraHandlerNotFoundException => EntraErrorCodes.HandlerNotFound,
                _ => EntraErrorCodes.ValidationError
            };

            return await ResponseAdapter.BadRequest(
                req,
                new EntraErrorResponse
                {
                    Error = code,
                    Details = ex.Message
                });
        }
        catch (Exception ex)
        {
            OnUnhandledException(ex, context);

            return await ResponseAdapter.ServerError(
                req,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.UnhandledException,
                    Details = "An unexpected error occurred."
                });
        }
    }

    protected abstract Task<HttpResponseData> Execute(HttpRequestData req, FunctionContext context);
}
