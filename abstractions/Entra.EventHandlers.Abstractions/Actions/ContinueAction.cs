using Entra.EventHandlers.Abstractions.Actions.Types;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action that instructs Microsoft Entra to continue the flow
/// using the default behavior for the current event. This action does not
/// modify attributes, interrupt the flow, or provide additional data.
/// </summary>
/// <remarks>
/// The concrete <c>@odata.type</c> value is provided by the
/// <see cref="ContinueActionType"/> passed to the constructor. This action is
/// supported only for events that define a continue‑with‑default‑behavior
/// contract, such as AttributeCollectionStart, AttributeCollectionSubmit,
/// and EmailOtpSend.
/// </remarks>
public sealed class ContinueAction(ContinueActionType type) : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, as defined by the
    /// <see cref="ContinueActionType"/> provided to the constructor.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = type.Value;
}
