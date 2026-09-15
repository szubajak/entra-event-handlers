using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.TestData;

public static class TestResponses
{
    public static AttributeCollectionStartResponse CreateAttributeCollectionStartResponse() =>
        new()
        {
            Data = new AttributeCollectionStartResponsePayload()
        };

    public static AttributeCollectionSubmitResponse CreateAttributeCollectionSubmitResponse() =>
        new()
        {
            Data = new AttributeCollectionSubmitResponsePayload()
        };

    public static EmailOtpSendResponse CreateEmailOtpSendResponse() =>
        new()
        {
            Data = new EmailOtpSendResponsePayload()
        };

    public static PasswordSubmitResponse CreatePasswordSubmitResponse(string nonce = "some-nonce") =>
        new()
        {
            Data = new PasswordSubmitResponsePayload
            { 
                Nonce = nonce
            }
        };

    public static TokenIssuanceStartResponse CreateTokenIssuanceStartResponse() =>
        new()
        {
            Data = new TokenIssuanceStartResponsePayload()
        };

    public static VerifiedIdClaimValidationResponse CreateVerifiedIdClaimValidationResponse() =>
        new()
        {
            Data = new VerifiedIdClaimValidationResponsePayload()
        };
}