using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.Authentication;

public class UserInfo
{
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("createdDateTime")]
    public DateTimeOffset? CreatedDateTime { get; set; }

    [JsonPropertyName("displayName")]
    public string? DiplayName { get; set; }

    [JsonPropertyName("givenName")]
    public string? GivenName { get; set; }

    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("mail")]
    public string? Mail { get; set; }

    [JsonPropertyName("onPremisesSamAccountName")]
    public string? OnPremisesSamAccountName { get; set; }

    [JsonPropertyName("onPremisesSecurityIdentifier")]
    public string? OnPremisesSecurityIdentifier { get; set; }

    [JsonPropertyName("onPremisesUserPrincipalName")]
    public string? OnPremisesUserPrincipalName { get; set; }

    [JsonPropertyName("preferredLanguage")]
    public string? PreferredLanguage { get; set; }

    [JsonPropertyName("preferredDataLocation")]
    public string? PreferredDataLocation { get; set; }

    [JsonPropertyName("surname")]
    public string? Surname { get; set; }

    [JsonPropertyName("userPrincipalName")]
    public string? UserPrincipalName { get; set; }

    [JsonPropertyName("userType")]
    public string? UserType { get; set; }
}
