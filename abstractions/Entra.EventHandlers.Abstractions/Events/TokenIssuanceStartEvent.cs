using Entra.EventHandlers.Abstractions.Protocol;

namespace Entra.EventHandlers.Abstractions.Events;

/// <summary>
/// Represents the incoming TokenIssuanceStart event sent by Microsoft Entra.
/// This event is triggered during token issuance and allows a custom extension
/// to add, modify, or remove claims before the token is returned to the client.
/// </summary>
/// <remarks>
/// For the official event schema and processing guidance, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-claims-provider-reference
/// </remarks>
public sealed class TokenIssuanceStartEvent : EntraEvent<TokenIssuanceStartEventPayload>
{
    public override string Type => EntraEventTypes.TokenIssuanceStart;
}

/// <summary>
/// Payload for the TokenIssuanceStart event.
/// Contains the authentication context, user information, and other data
/// that a custom extension may use to compute claims for the issued token.
///
/// This model mirrors the JSON structure expected by Microsoft Entra.
/// </summary>
/// <remarks>
/// For detailed event payload structure and supported claim operations, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-claims-provider-reference
/// </remarks>
public sealed class TokenIssuanceStartEventPayload : EntraEventPayload
{
    public override string OdataType { get; } = EntraOdataTypes.TokenIssuanceStart.CalloutData;
}