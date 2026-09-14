using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Endpoints;
using Entra.EventHandlers.AspNetCore.Interfaces;
using Entra.EventHandlers.Hosting.DI;

namespace Entra.EventHandlers.AspNetCore.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntraEventHandlers(this IServiceCollection services)
    {
        services.AddEntraEventHandlersHosting();

        services.AddSingleton<IRequestAdapter, RequestAdapter>()
                .AddSingleton<IResponseAdapter, ResponseAdapter>()
                .AddTransient<AttributeCollectionStartEndpoint>()
                .AddTransient<AttributeCollectionSubmitEndpoint>()
                .AddTransient<TokenIssuanceStartEndpoint>()
                .AddTransient<EmailOtpSendEndpoint>()
                .AddTransient<PasswordSubmitEndpoint>()
                .AddTransient<VerifiedIdClaimValidationEndpoint>()
                .AddTransient<EntraEventRouterEndpoint>();

        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo<IEntraExceptionHandler>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}
