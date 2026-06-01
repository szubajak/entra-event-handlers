using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.Authentication;

/// <summary>
/// Represents information about the user associated with the authentication
/// request. This data is provided by Microsoft Entra and mirrors selected
/// fields from the Microsoft Graph <c>user</c> resource.
/// </summary>
/// <remarks>
/// The properties included here may be used for personalization, validation,
/// conditional logic, or diagnostics within custom extension handlers.
/// Not all fields are guaranteed to be populated for every event.
/// </remarks>
public class UserInfo
{
    /// <summary>
    /// Gets or sets the company name associated with the user.
    /// </summary>
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; init; }

    /// <summary>
    /// Gets or sets the timestamp when the user object was created.
    /// </summary>
    [JsonPropertyName("createdDateTime")]
    public DateTimeOffset? CreatedDateTime { get; init; }

    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    [JsonPropertyName("displayName")]
    public string? DiplayName { get; init; }

    /// <summary>
    /// Gets or sets the given (first) name of the user.
    /// </summary>
    [JsonPropertyName("givenName")]
    public string? GivenName { get; init; }

    /// <summary>
    /// Gets or sets the object ID of the user in Microsoft Entra.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    /// <summary>
    /// Gets or sets the user's primary email address.
    /// </summary>
    [JsonPropertyName("mail")]
    public string? Mail { get; init; }

    /// <summary>
    /// Gets or sets the on-premises SAM account name, if the user is synced
    /// from Active Directory.
    /// </summary>
    [JsonPropertyName("onPremisesSamAccountName")]
    public string? OnPremisesSamAccountName { get; init; }

    /// <summary>
    /// Gets or sets the on-premises security identifier (SID) for the user,
    /// if applicable.
    /// </summary>
    [JsonPropertyName("onPremisesSecurityIdentifier")]
    public string? OnPremisesSecurityIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the on-premises user principal name (UPN), if the user is
    /// synchronized from Active Directory.
    /// </summary>
    [JsonPropertyName("onPremisesUserPrincipalName")]
    public string? OnPremisesUserPrincipalName { get; init; }

    /// <summary>
    /// Gets or sets the user's preferred language, typically in the form
    /// <c>language-region</c> (for example, <c>en-US</c>).
    /// </summary>
    [JsonPropertyName("preferredLanguage")]
    public string? PreferredLanguage { get; init; }

    /// <summary>
    /// Gets or sets the preferred data location for the user, such as
    /// <c>EUR</c> or <c>NA</c>.
    /// </summary>
    [JsonPropertyName("preferredDataLocation")]
    public string? PreferredDataLocation { get; init; }

    /// <summary>
    /// Gets or sets the user's surname (last name).
    /// </summary>
    [JsonPropertyName("surname")]
    public string? Surname { get; init; }

    /// <summary>
    /// Gets or sets the user's user principal name (UPN).
    /// </summary>
    [JsonPropertyName("userPrincipalName")]
    public string? UserPrincipalName { get; init; }

    /// <summary>
    /// Gets or sets the user type, such as <c>Member</c> or <c>Guest</c>.
    /// </summary>
    [JsonPropertyName("userType")]
    public string? UserType { get; init; }
}
