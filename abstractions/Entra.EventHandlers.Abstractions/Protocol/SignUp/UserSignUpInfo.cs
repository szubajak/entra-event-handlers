using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.SignUp;

/// <summary>
/// Represents the user sign-up information included in attribute collection
/// events. Contains the directory attributes and identity bindings provided
/// by Microsoft Entra during the sign-up flow.
/// </summary>
/// <remarks>
/// In the AttributeCollectionStart event, this object may contain pre-populated
/// values or be empty. In the AttributeCollectionSubmit event, it contains the
/// attribute values and identities submitted by the user.
///
/// The <c>attributes</c> dictionary includes directory attribute values keyed
/// by attribute name. The <c>identities</c> collection describes the sign-in
/// identities associated with the user.
/// </remarks>
public class UserSignUpInfo
{
    /// <summary>
    /// Gets or sets the directory attributes associated with the user, keyed
    /// by attribute name. Values include the raw attribute data and metadata
    /// provided by Microsoft Entra.
    /// </summary>
    [JsonPropertyName("attributes")]
    public Dictionary<string, DirectoryAttributeValue>? Attributes { get; set; }

    /// <summary>
    /// Gets or sets the collection of identities associated with the user,
    /// such as email, phone number, or external identity provider bindings.
    /// </summary>
    [JsonPropertyName("identities")]
    public IEnumerable<IdentityInfo>? Identities { get; set; }
}
