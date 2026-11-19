using System.Text;
using System.Text.Json;
using AutoFixture;
using Entra.EventHandlers.Observability.Clients;
using Entra.EventHandlers.Observability.Dtos;
using Entra.EventHandlers.Observability.UnitTests.Utils;
using FluentAssertions;

namespace Entra.EventHandlers.Observability.UnitTests.Clients;

public class ObservabilityApiClientTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task SendAsync_Success()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler();
        using var httpClient = new HttpClient(handler) { BaseAddress = _fixture.Create<Uri>() };

        var sut = new ObservabilityApiClient(httpClient);

        var eventLog = _fixture.Create<EventLogDto>();

        var json = JsonSerializer.Serialize(eventLog);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        await sut.SendAsync(eventLog);

        // Assert
        handler.Method.Should().Be(HttpMethod.Post);
        handler.RequestUri.Should().Be(ObservabilityApiClient.Path);

        handler.Body.Should().Be(await content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task SendAsync_TransportFails_PropagatesException()
    {
        // Arrange
        var message = _fixture.Create<string>();

        var handler = new FakeHttpMessageHandler(new HttpRequestException(message));
        using var httpClient = new HttpClient(handler) { BaseAddress = _fixture.Create<Uri>() };

        var sut = new ObservabilityApiClient(httpClient);

        var eventLog = _fixture.Create<EventLogDto>();

        // Act
        var act = () => sut.SendAsync(eventLog);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage(message);
    }
}
