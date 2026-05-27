using Entra.EventHandlers.Abstractions.Interfaces;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.SignUp;

public class DirectoryAttributeValue : IHaveOdataType
{
    [JsonPropertyName("@odata.type")]
    public required string OdataType { get; init; }

    [JsonPropertyName("value")]
    public object? Value { get; init; }

    [JsonPropertyName("attributeType")]
    public string? AttributeType { get; init; }
}
