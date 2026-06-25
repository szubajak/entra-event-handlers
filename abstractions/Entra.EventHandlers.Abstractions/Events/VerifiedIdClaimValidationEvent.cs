using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.VerifiedId;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Events;

/// <summary>
/// Represents the incoming VerifiedIdClaimValidation event sent by Microsoft Entra.
/// This event is triggered during the account recovery flow when a user presents
/// a Verified ID credential, allowing a custom extension to validate the claims
/// before the authentication process continues.
/// </summary>
/// <remarks>
/// For the official event schema and processing guidance, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-custom-authentication-extension-account-recovery
/// </remarks>
public sealed class VerifiedIdClaimValidationEvent : EntraEvent<VerifiedIdClaimValidationEventPayload>
{
    public override string Type => EntraEventTypes.VerifiedIdClaimValidation;
}

/// <summary>
/// Payload for the VerifiedIdClaimValidation event.
/// Contains the verified ID claims context, authentication context, and other
/// data that a custom extension may use to validate the credential and decide
/// whether the authentication flow should continue.
/// 
/// This model mirrors the JSON structure expected by Microsoft Entra.
/// </summary>
/// <remarks>
/// For detailed payload structure and validation behavior, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-custom-authentication-extension-account-recovery
/// </remarks>
public sealed class VerifiedIdClaimValidationEventPayload : EntraEventPayload
{
    public override string OdataType { get; } = EntraOdataTypes.VerifiedIdClaimValidation.CalloutData;

    /// <summary>
    /// Gets or sets the context containing the verified ID claims extracted
    /// from the credential and any additional metadata provided by the issuer.
    /// </summary>
    [JsonPropertyName("verifiedIdClaimsContext")]
    public VerifiedIdClaimsContext? VerifiedIdClaimsContext { get; init; }
}