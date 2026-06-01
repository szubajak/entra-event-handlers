using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action that modifies one or more attribute values submitted by
/// the user during the AttributeCollectionSubmit event. This action allows a
/// custom extension to transform, normalize, or override attribute values
/// before Microsoft Entra continues the sign-up flow.
/// </summary>
/// <remarks>
/// The <c>attributes</c> dictionary contains the updated attribute values keyed
/// by attribute name. Any attribute included in this dictionary replaces the
/// corresponding value submitted by the user. Attributes not included remain
/// unchanged.
///
/// This action is only valid in the AttributeCollectionSubmit response and is
/// ignored in other event types.
///
/// For details on modifying attribute values, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionsubmit-retrieve-return-data
/// </remarks>
public class ModifyAttributeValuesAction : EntraAction
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