using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.UnitTests.Utils;
using Entra.EventHandlers.UnitTests.Utils.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Handlers.Base;

public class AttributeCollectionStartHandlerBaseTests
{
    private readonly TestAttributeCollectionStartHandler _sut;

    private readonly TestLogger _logger;

    public AttributeCollectionStartHandlerBaseTests()
    {
        _logger = new TestLogger();

        _sut = new TestAttributeCollectionStartHandler(_logger);
    }

    [Fact]
    public async Task HandleAsync_Success()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateAttributeCollectionStartEvent(fixture);

        using var cts = new CancellationTokenSource();

        var expectedResponse = new AttributeCollectionStartResponse();
        _sut.ResponseToReturn = expectedResponse;

        // Act
        var response = await _sut.HandleAsync(evt, cts.Token);

        // Assert
        response.Should().Be(expectedResponse);

        _sut.CoreTest.HandleCoreCallCount.Should().Be(1);
        _sut.CoreTest.CapturedCancellationToken.Should().Be(cts.Token);

        _logger.Entries.Should().Contain(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("Handling event"));

        _logger.Entries.Should().Contain(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("Successfully handled event"));

        _logger.Scopes.Should().ContainSingle();

        var scope = (TestScope)_logger.Scopes.Single();
        var dict = (Dictionary<string, object?>)scope.State;

        dict.Should().ContainKey("CorrelationId").WhoseValue.Should().Be(evt.CorrelationId);
        dict.Should().ContainKey("EventType").WhoseValue.Should().Be(evt.Type);
        dict.Should().ContainKey("EventName").WhoseValue.Should().Be(nameof(AttributeCollectionStartEvent));
    }

    [Fact]
    public async Task HandleAsync_Fail()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateAttributeCollectionStartEvent(fixture);

        _sut.CoreTest.ShouldThrow = true;

        // Act
        var response = await _sut.HandleAsync(evt, CancellationToken.None);

        // Assert
        _sut.CoreTest.HandleCoreCallCount.Should().Be(1);

        _logger.Entries.Should().Contain(e =>
            e.Level == LogLevel.Error &&
            e.Message.Contains("Unhandled exception"));

        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ShowBlockPageAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.ShowBlockPage);
    }

    [Fact]
    public async Task HandleAsync_InvalidRequest()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateAttributeCollectionStartEvent(fixture, valid: false);

        // Act
        var response = await _sut.HandleAsync(evt, CancellationToken.None);

        // Assert
        _sut.CoreTest.HandleCoreCallCount.Should().Be(0);

        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ShowBlockPageAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.ShowBlockPage);
    }
}
