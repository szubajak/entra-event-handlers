using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.Resolvers;
public class TestResolver(IServiceProvider sp) : IEntraEventHandlerResolver
{
    private readonly IServiceProvider _sp = sp;

    public IEntraEventHandler Resolve(Type eventType)
    {
        return eventType.Name switch
        {
            nameof(AttributeCollectionStartEvent) => _sp.GetRequiredService<IAttributeCollectionStartHandler>(),
            nameof(AttributeCollectionSubmitEvent) => _sp.GetRequiredService<IAttributeCollectionSubmitHandler>(),
            nameof(TokenIssuanceStartEvent) => _sp.GetRequiredService<ITokenIssuanceStartHandler>(),
            nameof(EmailOtpSendEvent) => _sp.GetRequiredService<IEmailOtpSendHandler>(),
            nameof(PasswordSubmitEvent) => _sp.GetRequiredService<IPasswordSubmitHandler>(),
            _ => throw new EntraHandlerNotFoundException(eventType)
        };
    }
}
