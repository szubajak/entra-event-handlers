using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.Authentication;

namespace Entra.EventHandlers.Workforce.UnitTests.Utils;

public static class TestData
{
    public static VerifiedIdClaimValidationEvent CreateVerifiedIdClaimValidationEvent(IFixture fixture, bool valid = true) =>
        new()
        {
            Source = fixture.Create<string>(),
            Data = new VerifiedIdClaimValidationEventPayload
            {
                RawOdataType = valid ? EntraOdataTypes.VerifiedIdClaimValidation.CalloutData : "invalid",
                AuthenticationContext = fixture.Create<AuthenticationContext>()
            }
        };
}