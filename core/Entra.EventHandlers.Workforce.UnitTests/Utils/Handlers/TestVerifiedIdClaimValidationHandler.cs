using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.TestData;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.Workforce.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.Workforce.UnitTests.Utils.Handlers;

public class TestVerifiedIdClaimValidationHandler(ILogger logger)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public VerifiedIdClaimValidationResponse ResponseToReturn { get; set; } = TestResponses.CreateVerifiedIdClaimValidationResponse();

    protected override Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken)
    {
        CoreTest.Record(cancellationToken);
        return Task.FromResult(ResponseToReturn);
    }
}
