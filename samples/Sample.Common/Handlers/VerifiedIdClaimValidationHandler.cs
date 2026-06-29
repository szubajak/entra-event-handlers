using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Workforce.Builders;
using Entra.EventHandlers.Workforce.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Sample.Common.Handlers;

public class VerifiedIdClaimValidationHandler(ILogger<VerifiedIdClaimValidationHandler> logger)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
    protected override Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Pass()
                .Build());
    }
}