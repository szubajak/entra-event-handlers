using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using System.Net;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class EmailOtpSendEndpointTests(TestAppFactory factory) : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_EmailOtpSend_ShouldReachEndpoint()
    {
        // Arrange
        var payload = EventSamples.EmailOtpSend();

        // Act
        var response = await _client.PostAsync(
            "/emailotpsend",
            new StringContent(payload, Encoding.UTF8, "application/json"), 
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CancellationToken_ShouldBePassed()
    {
        // Arrange
        var payload = EventSamples.EmailOtpSend();

        var handler = factory.Services.GetRequiredService<TestEmailOtpSendHandler>();

        // Act
        await _client.PostAsync("/emailotpsend",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        // Assert
        handler.CapturedCancellationToken.CanBeCanceled.Should().BeTrue();
    }
}
