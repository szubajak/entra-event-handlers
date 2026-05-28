using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

/// <summary>
/// Represents an action that stops the attribute collection flow and returns
/// validation errors to the user. This action is used when one or more
/// submitted attribute values fail server-side validation.
/// </summary>
/// <remarks>
/// The <c>message</c> field provides a general validation error message shown
/// at the top of the page. The <c>attributeErrors</c> dictionary contains
/// per-attribute error messages keyed by attribute name.
///
/// This action is only valid in the AttributeCollectionSubmit response and is
/// ignored for other event types.
///
/// For details on returning validation errors, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionsubmit-retrieve-return-data
/// </remarks>
public class ShowValidationErrorAction : EntraAction
{
    /// <summary>
    /// Gets the OData type discriminator for the action, identifying it as a
    /// validation-error instruction in the Entra protocol.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionSubmit.ShowValidationError;

    /// <summary>
    /// Gets or sets the general validation message displayed to the user.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; set; }

    /// <summary>
    /// Gets or sets the collection of per-attribute validation errors, keyed
    /// by attribute name. Each entry provides a specific error message for the
    /// corresponding attribute.
    /// </summary>
    [JsonPropertyName("attributeErrors")]
    public Dictionary<string, string> AttributeErrors { get; set; } = [];
}