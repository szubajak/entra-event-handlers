using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action returned to Microsoft Entra during a
/// <see cref="TokenIssuanceStartEvent"/>. The action provides additional
/// claims to be included in the issued token, allowing a custom extension
/// to compute or override claims before Entra completes token issuance.
/// </summary>
/// <remarks>
/// The concrete <c>@odata.type</c> value is defined by the Entra protocol and
/// identifies this action as a provide‑claims instruction.
///
/// The <c>claims</c> dictionary contains the claim names and their values to be
/// added to the outgoing token. Any claim included in this dictionary replaces
/// an existing claim with the same name.
///
/// This action is valid only in the TokenIssuanceStart response.
///
/// For details on providing claims during token issuance, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-claims-provider-reference
/// </remarks>
public sealed class ProvideClaimsForTokenAction : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, identifying it as a
    /// provide-claims instruction in the Entra protocol.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.TokenIssuanceStart.ProvideClaimsForToken;

    /// <summary>
    /// Gets or sets the claims to add or override in the issued token, keyed
    /// by claim name. Values may be primitives or JSON-serializable objects.
    /// </summary>
    [JsonPropertyName("claims")]
    public Dictionary<string, object> Claims { get; init; } = [];
}