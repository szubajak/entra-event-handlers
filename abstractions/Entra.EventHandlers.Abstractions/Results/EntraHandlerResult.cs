using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Abstractions.Results;

public sealed record EntraHandlerResult<TResponse>(TResponse Response, Exception? Exception = null)
    where TResponse : EntraEventResponse
{
    public bool HasException => Exception is not null;
}
