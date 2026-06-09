using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Defines a handler for the PasswordSubmit event. Implementations process the
/// encrypted password context sent by Microsoft Entra and produce a valid
/// response according to the just‑in‑time password migration contract.
/// </summary>
/// <remarks>
/// The PasswordSubmit event is triggered when a user attempts to authenticate
/// with a password that must be evaluated by a custom extension. Handlers
/// decrypt the <c>encryptedPasswordContext</c>, validate or migrate the
/// password, and return an appropriate action such as migrate, update,
/// retry, or block.
///
/// For details on the PasswordSubmit event and the expected response schema,
/// see:
/// https://learn.microsoft.com/en-us/entra/external-id/customers/how-to-migrate-passwords-just-in-time
/// </remarks>
public interface IPasswordSubmitHandler
    : IEntraEventHandler<PasswordSubmitEvent, PasswordSubmitResponse>
{
}
