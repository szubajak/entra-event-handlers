using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using System.Net;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class EntraEventRouterEndpointHandlerNotFoundTests(TestAppFactoryPasswordSubmitHandlerNotFound factory) : IClassFixture<TestAppFactoryPasswordSubmitHandlerNotFound>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Router_ShouldReturnBadRequest_WhenHandlerNotFound()
    {
        // Arrange
        var payload = EventSamples.PasswordSubmit();

        // Act
        var response = await _client.PostAsync(
            "/router",
            new StringContent(payload, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("HandlerNotFound");
    }
}
