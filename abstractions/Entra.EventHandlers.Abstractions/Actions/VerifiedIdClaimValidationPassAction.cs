using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action returned to Microsoft Entra during a
/// <see cref="VerifiedIdClaimValidationEvent"/> indicating that all verified
/// ID claims passed validation. When this action is returned, Entra continues
/// the account recovery flow without interruption.
/// </summary>
/// <remarks>
/// The concrete <c>@odata.type</c> value identifies this action as a
/// successful‑claim‑validation instruction in the Entra protocol.
///
/// This action contains no additional properties. Its presence alone signals
/// that the verified ID credential is valid and the flow may proceed.
///
/// This action is valid only in the VerifiedIdClaimValidation response.
///
/// For details, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-custom-authentication-extension-account-recovery
/// </remarks>
public sealed class VerifiedIdClaimValidationPassAction : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, identifying it as a
    /// successful‑claim‑validation instruction in the Entra protocol.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.VerifiedIdClaimValidation.Pass;
}