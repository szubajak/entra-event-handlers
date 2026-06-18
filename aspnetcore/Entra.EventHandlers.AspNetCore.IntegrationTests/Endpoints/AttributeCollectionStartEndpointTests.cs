using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using System.Net;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class AttributeCollectionStartTests(TestAppFactory factory) : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_AttributeCollectionStart_ShouldReachEndpoint()
    {
        // Arrange
        var payload = EventSamples.AttributeCollectionStart();

        // Act
        var response = await _client.PostAsync(
            "/attributecollectionstart",
            new StringContent(payload, Encoding.UTF8, "application/json"), 
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CancellationToken_ShouldBePassed()
    {
        // Arrange
        var payload = EventSamples.AttributeCollectionStart();

        // Act
        var handler = factory.Services.GetRequiredService<TestAttributeCollectionStartHandler>();

        // Assert
        await _client.PostAsync("/attributecollectionstart",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        handler.CapturedCancellationToken.CanBeCanceled.Should().BeTrue();
    }
}
