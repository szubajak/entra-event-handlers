using Entra.EventHandlers.Abstractions.Interfaces;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Base type for all Microsoft Entra custom extension actions. Each derived
/// action represents a specific instruction that can be returned in a response
/// to influence the authentication, attribute collection, token issuance, OTP,
/// or password‑migration flow.
/// </summary>
/// <remarks>
/// Concrete action types define their own <c>@odata.type</c> discriminator and
/// payload structure according to the Microsoft Entra custom extension
/// contract. Actions may instruct Entra to continue the flow, display a block
/// page, prefill attribute values, return validation errors, modify claims
/// during token issuance, or control the just‑in‑time password migration flow
/// (for example: migrate, require reset, retry, or block).
///
/// For details on supported actions and response schemas, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-overview
/// </remarks>
[JsonDerivedType(typeof(ContinueAction))]
[JsonDerivedType(typeof(ShowBlockPageAction))]
[JsonDerivedType(typeof(SetPrefillValuesAction))]
[JsonDerivedType(typeof(ModifyAttributeValuesAction))]
[JsonDerivedType(typeof(ShowValidationErrorAction))]
[JsonDerivedType(typeof(ProvideClaimsForTokenAction))]
[JsonDerivedType(typeof(PasswordSubmitAction))]
public abstract class EntraAction : IHaveOdataType
{
    /// <summary>
    /// Gets the OData type discriminator for the action. Derived types override
    /// this value to match the action type expected by the Entra protocol.
    /// </summary>
    [JsonIgnore]
    public abstract string OdataType { get; }
}
