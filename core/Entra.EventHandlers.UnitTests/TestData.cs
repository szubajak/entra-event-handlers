using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.Authentication;
using Entra.EventHandlers.Abstractions.Protocol.Otp;

namespace Entra.EventHandlers.UnitTests;

public static class TestData
{
    public static EmailOtpSendEvent CreateEmailOtpSendEvent(IFixture fixture, bool valid = true) =>
        new()
        {
            Source = fixture.Create<string>(),
            Data = new EmailOtpSendEventPayload
            {
                RawOdataType = valid ? EntraOdataTypes.EmailOtpSend.CalloutData : "invalid",
                AuthenticationContext = fixture.Create<AuthenticationContext>(),
                OtpContext = fixture.Create<OtpContext>()
            }
        };
}