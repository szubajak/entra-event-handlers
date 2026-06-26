using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using System.Net;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class EntraEventRouterEndpointTests(TestAppFactory factory) : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [MemberData(nameof(RouterCases))]
    public async Task Router_ShouldRouteToCorrectHandler(Type handlerType, string payload)
    {
        // Arrange
        var handler = (TestHandlerBase)factory.Services.GetRequiredService(handlerType);

        // Act
        var response = await _client.PostAsync(
            "/router",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        handler.WasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Router_ShouldReturnBadRequest_WhenDeserializationFails()
    {
        // Arrange
        var invalidJson =
        """
        {
          "@odata.type": "invalidJson"
        """;

        // Act
        var response = await _client.PostAsync(
            "/router",
            new StringContent(invalidJson, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("DeserializationError");
    }

    [Fact]
    public async Task Router_ShouldReturnBadRequest_WhenUnknownEventType()
    {
        // Arrange
        var invalidJson =
        """
        {
          "@odata.type": "microsoft.graph.authenticationEvent.unknownEvent"
        }
        """;

        // Act
        var response = await _client.PostAsync(
            "/router",
            new StringContent(invalidJson, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("DeserializationError");
    }

    [Fact]
    public async Task Router_ShouldPassCancellationToken()
    {
        // Arrange
        var handler = factory.Services.GetRequiredService<TestPasswordSubmitHandler>();

        var payload = EventSamples.PasswordSubmit();

        // Act
        await _client.PostAsync(
            "/router",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        // Assert
        handler.CapturedCancellationToken.CanBeCanceled.Should().BeTrue();
    }

    public static TheoryData<Type, string> RouterCases() =>
        new()
        {
            { typeof(TestAttributeCollectionStartHandler), EventSamples.AttributeCollectionStart() },
            { typeof(TestAttributeCollectionSubmitHandler), EventSamples.AttributeCollectionSubmit() },
            { typeof(TestTokenIssuanceStartHandler), EventSamples.TokenIssuanceStart() },
            { typeof(TestEmailOtpSendHandler), EventSamples.EmailOtpSend() },
            { typeof(TestPasswordSubmitHandler), EventSamples.PasswordSubmit() }
        };
}
