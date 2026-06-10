namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Defines the initial stage of the response builder for the
/// PasswordSubmit event. This stage requires specifying the nonce
/// value that must be returned to Microsoft Entra before selecting
/// an action.
/// </summary>
public interface IPasswordSubmitResponseBuilderStart
{
    /// <summary>
    /// Sets the nonce value that Microsoft Entra provided in the
    /// <c>encryptedPasswordContext</c>. The same value must be returned
    /// in the response to maintain flow integrity.
    /// </summary>
    /// <param name="nonce">
    /// The nonce extracted from the incoming PasswordSubmit event payload.
    /// </param>
    IPasswordSubmitResponseBuilderActions WithNonce(string nonce);
}
