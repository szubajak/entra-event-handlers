using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.Workforce.Builders;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestVerifiedIdClaimValidationHandler : TestHandlerBase, IVerifiedIdClaimValidationHandler
{
    public Task<VerifiedIdClaimValidationResponse> HandleAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken = default)
    {
        request.Validate();

        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Pass()
                .Build());
    }
}
