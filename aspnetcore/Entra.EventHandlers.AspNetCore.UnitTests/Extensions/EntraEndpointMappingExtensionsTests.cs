using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Endpoints;
using Entra.EventHandlers.AspNetCore.Extensions;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.Hosting.Resolvers;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Extensions;

public sealed class EntraEndpointMappingExtensionsTests
{
    [Theory]
    [MemberData(nameof(EndpointsTypes))]
    public void Mapping_Should_Not_Throw(Type endpointType)
    {
        var builder = WebApplication.CreateBuilder();

        RegisterAllEndpoints(builder.Services);

        var app = builder.Build();

        var action = () =>
        {
            switch (endpointType)
            {
                case var t when t == typeof(AttributeCollectionStartEndpoint):
                    app.MapEntraAttributeCollectionStart();
                    break;

                case var t when t == typeof(AttributeCollectionSubmitEndpoint):
                    app.MapEntraAttributeCollectionSubmit();
                    break;

                case var t when t == typeof(EmailOtpSendEndpoint):
                    app.MapEntraEmailOtpSend();
                    break;

                case var t when t == typeof(PasswordSubmitEndpoint):
                    app.MapEntraPasswordSubmit();
                    break;

                case var t when t == typeof(TokenIssuanceStartEndpoint):
                    app.MapEntraTokenIssuanceStart();
                    break;

                case var t when t == typeof(VerifiedIdClaimValidationEndpoint):
                    app.MapEntraVerifiedIdClaimValidation();
                    break;

                case var t when t == typeof(EntraEventRouterEndpoint):
                    app.MapEntraRouter();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(endpointType));
            }
        };

        action.Should().NotThrow();
    }

    public static TheoryData<Type> EndpointsTypes() =>
        [
            typeof(AttributeCollectionStartEndpoint),
            typeof(AttributeCollectionSubmitEndpoint),
            typeof(EmailOtpSendEndpoint),
            typeof(PasswordSubmitEndpoint),
            typeof(TokenIssuanceStartEndpoint),
            typeof(VerifiedIdClaimValidationEndpoint),
            typeof(EntraEventRouterEndpoint),
        ];

    private static void RegisterAllEndpoints(IServiceCollection services)
    {
        services.AddSingleton(
            new AttributeCollectionStartEndpoint(
                Substitute.For<ILogger<AttributeCollectionStartEndpoint>>(),
                Substitute.For<IHttpAdapter>(),
                Substitute.For<IEntraEventHandlerResolver>()
            )
        );

        services.AddSingleton(
            new AttributeCollectionSubmitEndpoint(
                Substitute.For<ILogger<AttributeCollectionSubmitEndpoint>>(),
                Substitute.For<IHttpAdapter>(),
                Substitute.For<IEntraEventHandlerResolver>()
            )
        );

        services.AddSingleton(
            new EmailOtpSendEndpoint(
                Substitute.For<ILogger<EmailOtpSendEndpoint>>(),
                Substitute.For<IHttpAdapter>(),
                Substitute.For<IEntraEventHandlerResolver>()
            )
        );

        services.AddSingleton(
            new PasswordSubmitEndpoint(
                Substitute.For<ILogger<PasswordSubmitEndpoint>>(),
                Substitute.For<IHttpAdapter>(),
                Substitute.For<IEntraEventHandlerResolver>()
            )
        );

        services.AddSingleton(
            new TokenIssuanceStartEndpoint(
                Substitute.For<ILogger<TokenIssuanceStartEndpoint>>(),
                Substitute.For<IHttpAdapter>(),
                Substitute.For<IEntraEventHandlerResolver>()
            )
        );

        services.AddSingleton(
            new VerifiedIdClaimValidationEndpoint(
                Substitute.For<ILogger<VerifiedIdClaimValidationEndpoint>>(),
                Substitute.For<IHttpAdapter>(),
                Substitute.For<IEntraEventHandlerResolver>()
            )
        );

        services.AddSingleton(
            new EntraEventRouterEndpoint(
                Substitute.For<ILogger<EntraEventRouterEndpoint>>(),
                Substitute.For<IHttpAdapter>(),
                Substitute.For<IEntraEventOrchestrator>()
            )
        );
    }
}
