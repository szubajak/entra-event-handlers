using Entra.EventHandlers.Abstractions.Errors;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Security.Decryptors;

internal sealed class PasswordContextPayload
{
    [JsonPropertyName("user-password")]
    public required string Password { get; init; }

    [JsonPropertyName("nonce")]
    public required string Nonce { get; init; }

    [JsonPropertyName("username")]
    public string? Username { get; init; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(Password))
        {
            throw new EntraValidationException("The decrypted password context does not contain a valid password.");
        }

        if (string.IsNullOrWhiteSpace(Nonce))
        {
            throw new EntraValidationException("The decrypted password context does not contain a valid nonce.");
        }
    }
}
