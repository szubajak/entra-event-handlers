using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Defines a handler for the TokenIssuanceStart event.
/// Implementations process the incoming event and produce a valid response
/// according to the Microsoft Entra custom extension contract.
/// </summary>
/// <remarks>
/// The TokenIssuanceStart event is triggered during token issuance and allows
/// a custom extension to add, modify, or remove claims before the token is
/// returned to the client. Handlers may compute dynamic claims, enforce
/// conditional logic, or integrate with external systems.
///
/// For details on the TokenIssuanceStart event and the expected response
/// schema, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-claims-provider-reference
/// </remarks>
public interface ITokenIssuanceStartHandler
    : IEntraEventHandler<TokenIssuanceStartEvent, TokenIssuanceStartResponse>
{
}
