using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Base;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Utils;

public sealed class TestVerifiedIdClaimValidationFunctionBase(
    ILogger logger,
    IVerifiedIdClaimValidationHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : VerifiedIdClaimValidationFunctionBase(logger, handler, requestAdapter, responseAdapter)
{
    public bool ExceptionCalled { get; private set; }

    protected override Task OnExceptionAsync(Exception ex)
    {
        ExceptionCalled = true;
        return Task.CompletedTask;
    }
}
