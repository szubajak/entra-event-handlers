using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.Resolvers;

public class TestResolverPasswordSubmitHandlerNotFound : IEntraEventHandlerResolver
{
    public IEntraEventHandler<TEvent, TResponse> Resolve<TEvent, TResponse>()
        where TEvent : EntraEvent
        where TResponse : EntraEventResponse
    {
        if (typeof(TEvent) == typeof(PasswordSubmitEvent))
            throw new EntraHandlerNotFoundException(typeof(TEvent));

        throw new NotSupportedException(
            $"This test resolver only simulates missing PasswordSubmit handler. Unexpected event type: {typeof(TEvent).Name}");
    }
}
