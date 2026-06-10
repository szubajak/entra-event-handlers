using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action returned to Microsoft Entra during an
/// <see cref="AttributeCollectionSubmitEvent"/>. The action modifies one or
/// more attribute values submitted by the user before Entra continues the
/// attribute collection flow.
/// </summary>
/// <remarks>
/// The concrete <c>@odata.type</c> value is defined by the Entra protocol and
/// identifies this action as a modify‑attribute‑values instruction.  
///
/// The <c>attributes</c> dictionary contains the updated attribute values keyed
/// by attribute name. Any attribute included in this dictionary replaces the
/// corresponding value submitted by the user. Attributes not included remain
/// unchanged.
///
/// This action is valid only in the AttributeCollectionSubmit response.
///
/// For details, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionsubmit-retrieve-return-data
/// </remarks>
public sealed class ModifyAttributeValuesAction : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, identifying it as a
    /// modify-attribute-values instruction in the Entra protocol.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionSubmit.ModifyAttributeValues;

    /// <summary>
    /// Gets or sets the attribute values to modify, keyed by attribute name.
    /// Each entry replaces the corresponding value submitted by the user.
    /// </summary>
    [JsonPropertyName("attributes")]
    public Dictionary<string, object> Attributes { get; init; } = [];
}