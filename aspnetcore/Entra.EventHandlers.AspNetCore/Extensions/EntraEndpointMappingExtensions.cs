using Entra.EventHandlers.AspNetCore.Endpoints;

namespace Entra.EventHandlers.AspNetCore.Extensions;

public static class EntraEndpointMappingExtensions
{
    public static IEndpointRouteBuilder MapEntraAttributeCollectionStart(this IEndpointRouteBuilder endpoints)
    {
        var ep = endpoints.ServiceProvider.GetRequiredService<AttributeCollectionStartEndpoint>();
        ep.Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapEntraAttributeCollectionSubmit(this IEndpointRouteBuilder endpoints)
    {
        var ep = endpoints.ServiceProvider.GetRequiredService<AttributeCollectionSubmitEndpoint>();
        ep.Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapEntraTokenIssuanceStart(this IEndpointRouteBuilder endpoints)
    {
        var ep = endpoints.ServiceProvider.GetRequiredService<TokenIssuanceStartEndpoint>();
        ep.Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapEntraEmailOtpSend(this IEndpointRouteBuilder endpoints)
    {
        var ep = endpoints.ServiceProvider.GetRequiredService<EmailOtpSendEndpoint>();
        ep.Map(endpoints);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapEntraRouter(this IEndpointRouteBuilder endpoints)
    {
        var ep = endpoints.ServiceProvider.GetRequiredService<EntraEventRouterEndpoint>();
        ep.Map(endpoints);
        return endpoints;
    }
}