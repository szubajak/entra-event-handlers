using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.Workforce.Builders;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestVerifiedIdClaimValidationHandler : TestHandlerBase, IVerifiedIdClaimValidationHandler
{
    public Task<EntraHandlerResult<VerifiedIdClaimValidationResponse>> HandleAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken = default)
    {
        request.Validate();

        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        var response = EntraWorkforceEventResponses.VerifiedIdClaimValidation()
            .Pass()
            .Build();

        return Task.FromResult(new EntraHandlerResult<VerifiedIdClaimValidationResponse>(response));
    }
}
