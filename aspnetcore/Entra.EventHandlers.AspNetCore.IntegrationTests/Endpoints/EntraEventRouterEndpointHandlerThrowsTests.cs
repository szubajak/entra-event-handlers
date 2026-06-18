using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using System.Net;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class EntraEventRouterEndpointHandlerThrowsTests(TestAppFactoryPasswordSubmitHandlerThrows factory) : IClassFixture<TestAppFactoryPasswordSubmitHandlerThrows>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Router_ShouldReturnBadRequest_WhenHandlerThrowsEntraException()
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
        body.Should().Contain("ValidationError");
        body.Should().Contain("Invalid data!");
    }
}
