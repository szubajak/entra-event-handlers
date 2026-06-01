using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.Authentication;

/// <summary>
/// Represents information about the client application that initiated the
/// authentication request, including IP address, locale, and market.
/// </summary>
/// <remarks>
/// This data is provided by Microsoft Entra as part of the authentication
/// context and may be used for diagnostics, localization, or conditional logic
/// in custom extension handlers.
/// </remarks>
public class ClientInfo
{
    /// <summary>
    /// Gets or sets the IP address of the client that initiated the request.
    /// </summary>
    [JsonPropertyName("ip")]
    public string? Ip { get; init; }

    /// <summary>
    /// Gets or sets the locale of the client, typically in the form
    /// <c>language-region</c> (for example, <c>en-US</c>).
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; init; }

    /// <summary>
    /// Gets or sets the market associated with the client request, such as
    /// <c>en-US</c> or <c>pl-PL</c>.
    /// </summary>
    [JsonPropertyName("market")]
    public string? Market { get; init; }
}
