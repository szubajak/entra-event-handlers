using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.UnitTests.Utils;
using Entra.EventHandlers.UnitTests.Utils.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Handlers.Base;

public class AttributeCollectionSubmitHandlerBaseTests
{
    private readonly TestAttributeCollectionSubmitHandler _sut;

    private readonly TestLogger<AttributeCollectionSubmitHandlerBase> _logger = new();

    public AttributeCollectionSubmitHandlerBaseTests()
    {
        _sut = new TestAttributeCollectionSubmitHandler(_logger);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateAttributeCollectionSubmitEvent(fixture);

        using var cts = new CancellationTokenSource();

        var expectedResponse = new AttributeCollectionSubmitResponse();
        _sut.ResponseToReturn = expectedResponse;

        // Act
        var response = await _sut.Handle(evt, cts.Token);

        // Assert
        response.Should().Be(expectedResponse);

        _sut.CoreTest.HandleCoreCallCount.Should().Be(1);
        _sut.CoreTest.PassedCancellationToken.Should().Be(cts.Token);

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
        dict.Should().ContainKey("EventName").WhoseValue.Should().Be(nameof(AttributeCollectionSubmitEvent));
    }

    [Fact]
    public async Task Handle_Fail()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateAttributeCollectionSubmitEvent(fixture);

        _sut.CoreTest.ShouldThrow = true;

        // Act
        var response = await _sut.Handle(evt, CancellationToken.None);

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

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ShowBlockPage);
    }

    [Fact]
    public async Task Handle_InvalidRequest()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateAttributeCollectionSubmitEvent(fixture, valid: false);

        // Act
        var response = await _sut.Handle(evt, CancellationToken.None);

        // Assert
        _sut.CoreTest.HandleCoreCallCount.Should().Be(0);

        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ShowBlockPageAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ShowBlockPage);
    }
}
