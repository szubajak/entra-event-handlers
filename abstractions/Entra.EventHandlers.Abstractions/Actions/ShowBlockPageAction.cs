using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Events;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action returned to Microsoft Entra during an
/// <see cref="AttributeCollectionStartEvent"/> or
/// <see cref="AttributeCollectionSubmitEvent"/>. The action displays a block
/// page to the user and stops the current flow, presenting a custom title and
/// message that explain why the process cannot continue.
/// </summary>
/// <remarks>
/// The concrete <c>@odata.type</c> value is provided by the
/// <see cref="ShowBlockPageActionType"/> passed to the constructor.
///
/// The <c>title</c> and <c>message</c> fields define the content shown to the
/// user on the block page. This action immediately terminates the current
/// attribute collection step and prevents further progression.
///
/// This action is valid only in AttributeCollectionStart and
/// AttributeCollectionSubmit responses.
///
/// For details on block‑page actions, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-overview
/// </remarks>
public class ShowBlockPageAction(ShowBlockPageActionType type) : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, as defined by the
    /// <see cref="ShowBlockPageActionType"/> provided to the constructor.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = type.Value;

    /// <summary>
    /// Gets or sets the title displayed at the top of the block page.
    /// </summary>
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    /// <summary>
    /// Gets or sets the message shown to the user explaining why the flow
    /// cannot continue.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }
}
