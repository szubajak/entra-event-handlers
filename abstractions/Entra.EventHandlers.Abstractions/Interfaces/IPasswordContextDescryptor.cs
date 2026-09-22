using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;

namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Defines the contract for decrypting the <c>encryptedPasswordContext</c>
/// field of a <see cref="PasswordSubmitEvent"/>. Implementations are
/// responsible for performing JWE/JWT decryption using the extension's
/// private key and returning the plaintext password context.
/// </summary>
public interface IPasswordContextDecryptor
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
