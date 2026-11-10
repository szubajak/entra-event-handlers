using System.Net;
using System.Text;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestData;
using FluentAssertions;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class PasswordSubmitEndpointTests(TestAppFactory factory) : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_PasswordSubmit_ShouldReachEndpoint()
    {
        // Arrange
        var payload = EventSamples.PasswordSubmit();

        // Act
        var response = await _client.PostAsync(
            "/passwordsubmit",
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
        var payload = EventSamples.PasswordSubmit();

        var handler = factory.Services.GetRequiredService<TestPasswordSubmitHandler>();

        // Act
        await _client.PostAsync(
            "/passwordsubmit",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken
        );

        // Assert
        handler.CapturedCancellationToken.CanBeCanceled.Should().BeTrue();
    }
}
