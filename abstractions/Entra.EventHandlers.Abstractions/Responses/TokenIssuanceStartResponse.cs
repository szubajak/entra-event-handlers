using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

/// <summary>
/// Represents the response returned to Microsoft Entra for a
/// TokenIssuanceStart event.
///
/// Handlers use this type to construct a valid response according to the
/// Entra custom extension contract.
/// </summary>
/// <remarks>
/// For the official response schema and guidance, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-claims-provider-reference
/// </remarks>
public class TokenIssuanceStartResponse : EntraEventResponse<TokenIssuanceStartResponsePayload>
{
}

/// <summary>
/// Payload for the response to a TokenIssuanceStart event.
/// Specifies the claims and actions that Microsoft Entra should apply
/// during token issuance.
///
/// This model mirrors the JSON structure expected by Microsoft Entra.
/// </summary>
/// <remarks>
/// For detailed response structure, claim definitions, and supported actions,
/// see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-claims-provider-reference
/// </remarks>
public class TokenIssuanceStartResponsePayload : EntraEventResponsePayload
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.TokenIssuanceStart.ResponseData;
}