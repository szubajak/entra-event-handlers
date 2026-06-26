using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
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

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task HandleAsync_Success(bool withAction)
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateEmailOtpSendEvent(fixture);

        using var cts = new CancellationTokenSource();

        var expectedResponse = new EmailOtpSendResponse
        {
            Data = new EmailOtpSendResponsePayload
            {
                Actions = withAction
                ? new List<EntraAction>
                {
                    new ContinueAction(ContinueActionType.EmailOtpSendContinueWithDefaultBehavior)
                }
                : Array.Empty<EntraAction>()
            }
        };

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

        var success = _logger.Entries.Single(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("Successfully handled event"));

        var state = success.State.As<IReadOnlyList<KeyValuePair<string, object>>>();

        var logged = state.Single(kv => kv.Key == "ActionType").Value?.ToString();

        var expected = withAction
            ? EntraOdataTypes.EmailOtpSend.ContinueWithDefaultBehavior
            : "None";

        logged.Should().Be(expected);

        _logger.Scopes.Should().ContainSingle();

        var scope = (TestScope)_logger.Scopes.Single();
        var dict = (Dictionary<string, object?>)scope.State;

        dict.Should().ContainKey("CorrelationId").WhoseValue.Should().Be(evt.CorrelationId);
        dict.Should().ContainKey("EventType").WhoseValue.Should().Be(evt.Type);
        dict.Should().ContainKey("EventName").WhoseValue.Should().Be(nameof(EmailOtpSendEvent));
    }

    [Fact]
    public async Task HandleAsync_Fail()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateEmailOtpSendEvent(fixture);

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
            .BeOfType<ContinueAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.EmailOtpSend.ContinueWithDefaultBehavior);
    }

    [Fact]
    public async Task HandleAsync_InvalidRequest()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateEmailOtpSendEvent(fixture, valid: false);

        // Act
        var response = await _sut.HandleAsync(evt, CancellationToken.None);

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
