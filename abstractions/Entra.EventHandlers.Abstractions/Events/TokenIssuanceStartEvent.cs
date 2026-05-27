using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Responses;
using Mediator;

namespace Entra.EventHandlers.Abstractions.Events;

public class TokenIssuanceStartEvent : EntraEvent<TokenIssuanceStartEventPayload>, IRequest<TokenIssuanceStartResponse>
{
    public override string Type => EntraEventTypes.TokenIssuanceStart;
}

public class TokenIssuanceStartEventPayload : EntraEventPayload
{
    public override string OdataType { get; } = EntraOdataTypes.TokenIssuanceStart.CalloutData;
}