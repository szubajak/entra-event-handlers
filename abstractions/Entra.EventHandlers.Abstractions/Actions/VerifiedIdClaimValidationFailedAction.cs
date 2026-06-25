using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action returned to Microsoft Entra during a
/// <see cref="VerifiedIdClaimValidationEvent"/> indicating that one or more
/// verified ID claims failed validation. When this action is returned, Entra
/// stops the account recovery flow and reports the failed claims.
/// </summary>
/// <remarks>
/// The concrete <c>@odata.type</c> value identifies this action as a
/// failed‑claim‑validation instruction in the Entra protocol.
///
/// The <c>failedClaims</c> collection specifies which verified ID claims did
/// not pass validation. Microsoft Entra uses this information to determine
/// why the credential cannot be accepted and to halt the recovery process.
///
/// This action is valid only in the VerifiedIdClaimValidation response.
///
/// For details, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-custom-authentication-extension-account-recovery
/// </remarks>
public sealed class VerifiedIdClaimValidationFailedAction : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, identifying it as a
    /// failed‑claim‑validation instruction in the Entra protocol.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.VerifiedIdClaimValidation.Failed;

    /// <summary>
    /// Gets or sets the list of verified ID claims that failed validation.
    /// Each entry corresponds to a claim name present in the credential.
    /// </summary>
    [JsonPropertyName("failedClaims")]
    public IReadOnlyList<string> FailedClaims { get; init; } = [];
}
