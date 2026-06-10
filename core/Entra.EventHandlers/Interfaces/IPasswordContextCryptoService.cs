using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Protocol.PasswordSubmit;

namespace Entra.EventHandlers.Interfaces;

/// <summary>
/// Provides cryptographic operations for decrypting the
/// <c>encryptedPasswordContext</c> field of a <see cref="PasswordSubmitEvent"/>.
/// Implementations are responsible for performing JWE/JWT decryption using
/// the extension’s private key.
/// </summary>
public interface IPasswordContextCryptoService
{
    /// <summary>
    /// Decrypts the encrypted password context provided by Microsoft Entra
    /// and returns the plaintext password, nonce, and optional username.
    /// </summary>
    /// <param name="encryptedPasswordContext">
    /// The encrypted value from the event payload.
    /// </param>
    /// <returns>
    /// A <see cref="DecryptedPasswordContext"/> containing the decrypted
    /// password, nonce, and username.
    /// </returns>
    DecryptedPasswordContext Decrypt(string encryptedPasswordContext);
}
