using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.Resolvers;

public class TestResolverPasswordSubmitHandlerNotFound : IEntraEventHandlerResolver
{
    public IEntraEventHandler Resolve(Type eventType)
    {
        if (eventType == typeof(PasswordSubmitEvent))
            throw new EntraHandlerNotFoundException(eventType);

        throw new NotSupportedException();
    }
}
