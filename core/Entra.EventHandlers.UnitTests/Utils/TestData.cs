using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.Authentication;
using Entra.EventHandlers.Abstractions.Protocol.Otp;
using Entra.EventHandlers.Abstractions.Protocol.SignUp;

namespace Entra.EventHandlers.UnitTests.Utils;

public static class TestData
{
    public static AttributeCollectionStartEvent CreateAttributeCollectionStartEvent(IFixture fixture, bool valid = true) =>
        new()
        {
            Source = fixture.Create<string>(),
            Data = new AttributeCollectionStartEventPayload
            {
                RawOdataType = valid ? EntraOdataTypes.AttributeCollectionStart.CalloutData : "invalid",
                AuthenticationContext = fixture.Create<AuthenticationContext>()
            }
        };

    public static AttributeCollectionSubmitEvent CreateAttributeCollectionSubmitEvent(IFixture fixture, bool valid = true) =>
        new()
        {
            Source = fixture.Create<string>(),
            Data = new AttributeCollectionSubmitEventPayload
            {
                RawOdataType = valid ? EntraOdataTypes.AttributeCollectionSubmit.CalloutData : "invalid",
                AuthenticationContext = fixture.Create<AuthenticationContext>(),
                UserSignUpInfo = fixture.Create<UserSignUpInfo>()
            }
        };

    public static TokenIssuanceStartEvent CreateTokenIssuanceStartEvent(IFixture fixture, bool valid = true) =>
        new()
        {
            Source = fixture.Create<string>(),
            Data = new TokenIssuanceStartEventPayload
            {
                RawOdataType = valid ? EntraOdataTypes.TokenIssuanceStart.CalloutData : "invalid",
                AuthenticationContext = fixture.Create<AuthenticationContext>()
            }
        };

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

    public static PasswordSubmitEvent CreatePasswordSubmitEvent(IFixture fixture, bool valid = true) =>
        new()
        {
            Source = fixture.Create<string>(),
            Data = new PasswordSubmitEventPayload
            {
                RawOdataType = valid ? EntraOdataTypes.PasswordSubmit.CalloutData : "invalid",
                AuthenticationContext = fixture.Create<AuthenticationContext>(),
                EncryptedPasswordContext = fixture.Create<string>()
            }
        };
}