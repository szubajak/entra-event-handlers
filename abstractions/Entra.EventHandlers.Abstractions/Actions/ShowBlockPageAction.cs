using Entra.EventHandlers.Abstractions.Actions.Types;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action that interrupts the flow and displays a block page to
/// the user. This action is used to stop the sign-up or attribute collection
/// process and present a custom message explaining why the flow cannot
/// continue.
/// </summary>
/// <remarks>
/// The <c>title</c> and <c>message</c> fields define the content shown to the
/// user. The concrete <c>@odata.type</c> value is provided by the
/// <see cref="ShowBlockPageActionType"/> passed to the constructor.
///
/// This action is valid in both AttributeCollectionStart and
/// AttributeCollectionSubmit responses.
///
/// For details on block page actions, see:
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
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the message shown to the user explaining why the flow
    /// cannot continue.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}
