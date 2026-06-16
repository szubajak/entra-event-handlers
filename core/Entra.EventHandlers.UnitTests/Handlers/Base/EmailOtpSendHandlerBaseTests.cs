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

public class EmailOtpSendHandlerBaseTests
{
    private readonly TestEmailOtpSendHandler _sut;

    private readonly TestLogger _logger;

    public EmailOtpSendHandlerBaseTests()
    {
        _logger = new TestLogger();

        _sut = new TestEmailOtpSendHandler(_logger);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateEmailOtpSendEvent(fixture);

        using var cts = new CancellationTokenSource();

        var expectedResponse = new EmailOtpSendResponse();
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
        dict.Should().ContainKey("EventName").WhoseValue.Should().Be(nameof(EmailOtpSendEvent));
    }

    [Fact]
    public async Task Handle_Fail()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateEmailOtpSendEvent(fixture);

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
            .BeOfType<ContinueAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.EmailOtpSend.ContinueWithDefaultBehavior);
    }

    [Fact]
    public async Task Handle_InvalidRequest()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateEmailOtpSendEvent(fixture, valid: false);

        // Act
        var response = await _sut.Handle(evt, CancellationToken.None);

        // Assert
        _sut.CoreTest.HandleCoreCallCount.Should().Be(0);

        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ContinueAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.EmailOtpSend.ContinueWithDefaultBehavior);
    }
}
