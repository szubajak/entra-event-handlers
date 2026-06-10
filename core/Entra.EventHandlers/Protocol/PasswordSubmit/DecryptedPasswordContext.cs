namespace Entra.EventHandlers.Protocol.PasswordSubmit;

/// <summary>
/// Represents the decrypted contents of the <c>encryptedPasswordContext</c>
/// field from a <see cref="PasswordSubmitEvent"/>. This model contains the
/// plaintext password, nonce, and optional username extracted by the handler
/// pipeline before invoking event‑specific logic.
/// </summary>
/// <remarks>
/// These values are decrypted by the runtime and are never part of the
/// incoming event payload. The password should be treated as highly sensitive
/// and kept in memory only for the duration of the request.
/// </remarks>
public sealed class DecryptedPasswordContext
{
    /// <summary>
    /// The plaintext password submitted by the user. This value is decrypted
    /// from the <c>encryptedPasswordContext</c> and should never be logged or
    /// persisted.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// The nonce provided by Microsoft Entra. This value must be returned
    /// unchanged in the <see cref="PasswordSubmitResponse"/> to ensure
    /// protocol integrity.
    /// </summary>
    public required string Nonce { get; init; }

    /// <summary>
    /// The username associated with the password, if present in the decrypted
    /// context. This value may be null depending on the tenant configuration.
    /// </summary>
    public required string Username { get; init; }
}
