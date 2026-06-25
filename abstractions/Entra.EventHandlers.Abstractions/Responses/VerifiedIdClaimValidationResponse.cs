using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

/// <summary>
/// Represents the response returned to Microsoft Entra for a
/// VerifiedIdClaimValidation event.
///
/// Handlers use this type to construct a valid response according to the
/// Entra custom extension contract.
/// </summary>
/// <remarks>
/// For the official response schema and guidance, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-custom-authentication-extension-account-recovery
/// </remarks>
public sealed class VerifiedIdClaimValidationResponse : EntraEventResponse<VerifiedIdClaimValidationResponsePayload>
{
}

/// <summary>
/// Payload for the response to a VerifiedIdClaimValidation event.
/// Specifies the action Microsoft Entra should take based on the result of
/// the verified ID claim validation, such as allowing the flow to continue
/// or blocking the authentication attempt.
///
/// This model mirrors the JSON structure expected by Microsoft Entra.
/// </summary>
/// <remarks>
/// For detailed response structure and supported validation actions, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-custom-authentication-extension-account-recovery
/// </remarks>
public sealed class VerifiedIdClaimValidationResponsePayload : EntraEventResponsePayload
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.VerifiedIdClaimValidation.ResponseData;
}