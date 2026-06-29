using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using System.Net;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class VerifiedIdClaimValidationEndpointTests(TestAppFactory factory) : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_VerifiedIdClaimValidation_ShouldReachEndpoint()
    {
        // Arrange
        var payload = EventSamples.VerifiedIdClaimValidation();

        // Act
        var response = await _client.PostAsync(
            "/verifiedidclaimvalidation",
            new StringContent(payload, Encoding.UTF8, "application/json"), 
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CancellationToken_ShouldBePassed()
    {
        // Arrange
        var payload = EventSamples.VerifiedIdClaimValidation();

        var handler = factory.Services.GetRequiredService<TestVerifiedIdClaimValidationHandler>();

        // Act
        await _client.PostAsync("/verifiedidclaimvalidation",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        // Assert
        handler.CapturedCancellationToken.CanBeCanceled.Should().BeTrue();
    }
}
