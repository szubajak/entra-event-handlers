using Entra.EventHandlers.Abstractions.Interfaces;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.SignUp;

/// <summary>
/// Represents a typed directory attribute value used in Microsoft Entra
/// attribute collection events. Each attribute includes an OData type
/// discriminator, the raw value, and metadata describing the attribute source.
/// </summary>
/// <remarks>
/// This model corresponds to the directory attribute value objects returned by
/// Microsoft Entra, such as:
/// <c>microsoft.graph.stringDirectoryAttributeValue</c>,
/// <c>microsoft.graph.int64DirectoryAttributeValue</c>, and others.
///
/// The <c>@odata.type</c> field identifies the concrete attribute value type.
/// The <c>value</c> property contains the raw attribute value, which may be
/// deserialized as a <see cref="System.Text.Json.JsonElement"/>.
/// </remarks>
public class DirectoryAttributeValue : IHaveOdataType
{
    /// <summary>
    /// Gets the OData type discriminator for the directory attribute value.
    /// This identifies the concrete attribute type defined by Microsoft Entra.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public required string OdataType { get; init; }

    /// <summary>
    /// Gets the raw value of the directory attribute. The value may be a
    /// primitive type or a JSON element depending on the attribute definition.
    /// </summary>
    [JsonPropertyName("value")]
    public object? Value { get; init; }

    /// <summary>
    /// Gets the attribute category, such as <c>builtIn</c> or
    /// <c>directorySchemaExtension</c>, indicating the source of the attribute.
    /// </summary>
    [JsonPropertyName("attributeType")]
    public string? AttributeType { get; init; }
}
