using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action that provides additional claims to be included in the
/// token during the TokenIssuanceStart event. This action allows a custom
/// extension to compute or override claims before Microsoft Entra issues the
/// final token.
/// </summary>
/// <remarks>
/// The <c>claims</c> dictionary contains the claim names and their values to be
/// added to the token. Any claim included in this dictionary is merged into the
/// outgoing token, replacing existing values if the claim already exists.
///
/// This action is only valid in the TokenIssuanceStart response and is ignored
/// for other event types.
///
/// For details on providing claims during token issuance, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-claims-provider-reference
/// </remarks>
public class ProvideClaimsForTokenAction : EntraAction
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