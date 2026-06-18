using Entra.EventHandlers.AspNetCore.Endpoints;

namespace Entra.EventHandlers.AspNetCore.Extensions;

public static class EntraEndpointMappingExtensions
{
    public static IEndpointRouteBuilder MapEntraAttributeCollectionStart(this IEndpointRouteBuilder endpoints)
    {
        endpoints.ServiceProvider.GetRequiredService<AttributeCollectionStartEndpoint>().Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapEntraAttributeCollectionSubmit(this IEndpointRouteBuilder endpoints)
    {
        endpoints.ServiceProvider.GetRequiredService<AttributeCollectionSubmitEndpoint>().Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapEntraTokenIssuanceStart(this IEndpointRouteBuilder endpoints)
    {
        endpoints.ServiceProvider.GetRequiredService<TokenIssuanceStartEndpoint>().Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapEntraEmailOtpSend(this IEndpointRouteBuilder endpoints)
    {
        endpoints.ServiceProvider.GetRequiredService<EmailOtpSendEndpoint>().Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapPasswordSubmit(this IEndpointRouteBuilder endpoints)
    {
        endpoints.ServiceProvider.GetRequiredService<PasswordSubmitEndpoint>().Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapEntraRouter(this IEndpointRouteBuilder endpoints)
    {
        endpoints.ServiceProvider.GetRequiredService<EntraEventRouterEndpoint>().Map(endpoints);
        return endpoints;
    }
}