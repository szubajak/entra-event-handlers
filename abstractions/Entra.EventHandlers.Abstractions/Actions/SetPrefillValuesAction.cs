using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action returned to Microsoft Entra during an
/// <see cref="AttributeCollectionStartEvent"/>. The action provides prefilled
/// attribute values that are shown to the user as initial input when the
/// attribute collection flow begins.
/// </summary>
/// <remarks>
/// The concrete <c>@odata.type</c> value is defined by the Entra protocol and
/// identifies this action as a set‑prefill‑values instruction.
///
/// The <c>inputs</c> dictionary contains attribute names and their corresponding
/// prefilled values. Any attribute included in this dictionary is displayed to
/// the user with the provided value as the initial input.
///
/// This action is valid only in the AttributeCollectionStart response.
///
/// For details on pre‑filling attribute values, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionstart-retrieve-return-data
/// </remarks>
public class SetPrefillValuesAction : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, identifying it as a
    /// set-prefill-values instruction in the Entra protocol.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionStart.SetPrefillValues;

    /// <summary>
    /// Gets or sets the prefilled attribute values, keyed by attribute name.
    /// These values are presented to the user as initial input during the
    /// attribute collection flow.
    /// </summary>
    [JsonPropertyName("inputs")]
    public Dictionary<string, object> Inputs { get; init; } = [];
}