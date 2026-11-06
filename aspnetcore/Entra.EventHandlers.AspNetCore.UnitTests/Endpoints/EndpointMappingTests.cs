using System.Reflection;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Endpoints;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.Hosting.Resolvers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Endpoints;

public class EndpointMappingTests
{
    public static TheoryData<Type, string> Routes() =>
        new()
        {
            { typeof(AttributeCollectionStartEndpoint), "attributecollectionstart" },
            { typeof(AttributeCollectionSubmitEndpoint), "attributecollectionsubmit" },
            { typeof(EmailOtpSendEndpoint), "emailotpsend" },
            { typeof(PasswordSubmitEndpoint), "passwordsubmit" },
            { typeof(TokenIssuanceStartEndpoint), "tokenissuancestart" },
            { typeof(VerifiedIdClaimValidationEndpoint), "verifiedidclaimvalidation" },
        };

    [Theory]
    [MemberData(nameof(Routes))]
    public void Endpoint_Should_Use_Expected_Route(Type endpointType, string expectedRoute)
    {
        // Arrange
        var endpoint = CreateEndpoint(endpointType);

        // Act
        var routeProperty = endpoint.GetType().BaseType!.GetProperty("Route", BindingFlags.Instance | BindingFlags.NonPublic);

        var route = (string)routeProperty!.GetValue(endpoint)!;

        // Assert
        route.Should().Be(expectedRoute);
    }

    [Fact]
    public void RouterEndpoint_Should_Use_Expected_Route()
    {
        // Arrange
        var endpoint = Activator.CreateInstance(
            typeof(EntraEventRouterEndpoint),
            Substitute.For<ILogger<EntraEventRouterEndpoint>>(),
            Substitute.For<IHttpAdapter>(),
            Substitute.For<IEntraEventOrchestrator>()
        )!;

        // Act
        var routeProperty = endpoint.GetType().BaseType!.GetProperty("Route", BindingFlags.Instance | BindingFlags.NonPublic);

        var route = (string)routeProperty!.GetValue(endpoint)!;

        // Assert
        route.Should().Be("router");
    }

    private static object CreateEndpoint(Type endpointType)
    {
        var loggerType = typeof(ILogger<>).MakeGenericType(endpointType);

        return Activator.CreateInstance(
            endpointType,
            Substitute.For([loggerType], []),
            Substitute.For<IHttpAdapter>(),
            Substitute.For<IEntraEventHandlerResolver>()
        )!;
    }
}
