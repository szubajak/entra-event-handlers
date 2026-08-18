using AutoFixture;
using Entra.EventHandlers.Observability.Clients;
using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Dtos;
using Entra.EventHandlers.Observability.Logging;
using Entra.EventHandlers.Observability.Mappers;
using Entra.EventHandlers.Observability.Models;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Entra.EventHandlers.Observability.UnitTests.Logging;

public class EventLogPublisherTests
{
    private readonly EventLogPublisher _sut;

    private readonly Fixture _fixture = new();
    private readonly IObservabilityApiClient _client;
    private readonly IEventLogContextMapper _mapper;

    public EventLogPublisherTests()
    {
        _client = Substitute.For<IObservabilityApiClient>();
        _mapper = Substitute.For<IEventLogContextMapper>();

        _sut = new EventLogPublisher(_client, _mapper);
    }

    [Fact]
    public async Task Publish_Success()
    {
        // Arrange
        var ctx = new EventLogContext
        {
            DefaultLog = _fixture.Create<EventLogEntry>()
        };

        var expectedDto = _fixture.Create<EventLogDto>();

        _mapper
            .Map(ctx)
            .Returns(expectedDto);

        // Act
        _sut.Publish(ctx);

        await Task.Delay(500, TestContext.Current.CancellationToken);

        // Assert
        _ = _client.Received(1).SendAsync(expectedDto);
    }

    [Fact]
    public void Publish_Should_Not_Throw_When_ApiClient_Fails()
    {
        // Arrange
        var ctx = new EventLogContext
        {
            DefaultLog = _fixture.Create<EventLogEntry>()
        };

        var expectedDto = _fixture.Create<EventLogDto>();

        _mapper
            .Map(ctx)
            .Returns(expectedDto);

        _client
            .SendAsync(expectedDto)
            .ThrowsAsync<Exception>();

        // Act
        var exception = Record.Exception(() => _sut.Publish(ctx));

        // Assert
        exception.Should().BeNull();
    }
}
