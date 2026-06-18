using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using System.Net;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class EntraEventRouterEndpointHandlerThrowsUnexpectedTests(TestAppFactoryPasswordSubmitHandlerThrowsUnexpected factory) : IClassFixture<TestAppFactoryPasswordSubmitHandlerThrowsUnexpected>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Router_ShouldReturnServerError_WhenHandlerThrowsUnexpectedException()
    {
        // Arrange
        var payload = EventSamples.PasswordSubmit();

        // Act
        var response = await _client.PostAsync(
            "/router",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("UnhandledException");
        body.Should().Contain("An unexpected error occurred.");
    }
}
