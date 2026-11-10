using System.Net;
using System.Text;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestData;
using FluentAssertions;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class TokenIssuanceStartEndpointTests(TestAppFactory factory) : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_TokenIssuanceStart_ShouldReachEndpoint()
    {
        // Arrange
        var payload = EventSamples.TokenIssuanceStart();

        // Act
        var response = await _client.PostAsync(
            "/tokenissuancestart",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ExecuteAsync_CancellationTokenProvided_PassesCancellationToken()
    {
        // Arrange
        var payload = EventSamples.TokenIssuanceStart();

        var handler = factory.Services.GetRequiredService<TestTokenIssuanceStartHandler>();

        // Act
        await _client.PostAsync(
            "/tokenissuancestart",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken
        );

        // Assert
        handler.CapturedCancellationToken.CanBeCanceled.Should().BeTrue();
    }
}
