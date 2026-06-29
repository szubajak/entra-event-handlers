using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.Resolvers;
public class TestResolver(IServiceProvider sp) : IEntraEventHandlerResolver
{
    private readonly IServiceProvider _sp = sp;

    public IEntraEventHandler<TEvent, TResponse> Resolve<TEvent, TResponse>()
        where TEvent : EntraEvent
        where TResponse : EntraEventResponse
    {
        var handler = ResolveUntyped<TEvent>();

        return handler as IEntraEventHandler<TEvent, TResponse>
            ?? throw new InvalidOperationException(
                $"Registered handler does not implement IEntraEventHandler<{typeof(TEvent).Name}, {typeof(TResponse).Name}>");
    }

    private IEntraEventHandler ResolveUntyped<TEvent>()
        where TEvent : EntraEvent =>
        typeof(TEvent) switch
        {
            Type t when t == typeof(AttributeCollectionStartEvent) =>
                _sp.GetRequiredService<IAttributeCollectionStartHandler>(),

            Type t when t == typeof(AttributeCollectionSubmitEvent) =>
                _sp.GetRequiredService<IAttributeCollectionSubmitHandler>(),

            Type t when t == typeof(TokenIssuanceStartEvent) =>
                _sp.GetRequiredService<ITokenIssuanceStartHandler>(),

            Type t when t == typeof(EmailOtpSendEvent) =>
                _sp.GetRequiredService<IEmailOtpSendHandler>(),

            Type t when t == typeof(PasswordSubmitEvent) =>
                _sp.GetRequiredService<IPasswordSubmitHandler>(),

            Type t when t == typeof(VerifiedIdClaimValidationEvent) =>
                _sp.GetRequiredService<IVerifiedIdClaimValidationHandler>(),

            _ => throw new EntraHandlerNotFoundException(typeof(TEvent))
        };
}
