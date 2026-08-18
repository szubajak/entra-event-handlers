using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Observability.Models;

namespace Entra.EventHandlers.Observability.Interfaces;

public interface IEventLogMapper<TRequest, TResponse>
    where TRequest : EntraEvent
    where TResponse : EntraEventResponse
{
    EventLogEntry Map(TRequest request, TResponse response);
}