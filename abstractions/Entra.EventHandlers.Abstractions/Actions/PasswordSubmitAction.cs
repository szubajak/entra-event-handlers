using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Events;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action returned to Microsoft Entra during a
/// <see cref="PasswordSubmitEvent"/>. The action instructs Entra how to
/// proceed after evaluating the submitted password—for example, migrating
/// the password, requiring a reset, retrying submission, or blocking the
/// attempt.
/// </summary>
/// <remarks>
/// The concrete <c>@odata.type</c> value is provided by the
/// <see cref="PasswordSubmitActionType"/> passed to the constructor.
/// Each action corresponds to a specific outcome in the password migration
/// flow as defined by the Microsoft Entra protocol.
/// </remarks>
public sealed class PasswordSubmitAction(PasswordSubmitActionType type) : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, as defined by the
    /// <see cref="PasswordSubmitActionType"/> provided to the constructor.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = type.Value;
}
