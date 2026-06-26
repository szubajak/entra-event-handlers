using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.Workforce.UnitTests.Utils;
using Entra.EventHandlers.Workforce.UnitTests.Utils.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.Workforce.UnitTests.Handlers.Base;

public class VerifiedIdClaimValidationHandlerBaseTests
{
    private readonly TestVerifiedIdClaimValidationHandler _sut;

    private readonly TestLogger _logger;

    public VerifiedIdClaimValidationHandlerBaseTests()
    {
        _logger = new TestLogger();

        _sut = new TestVerifiedIdClaimValidationHandler(_logger);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task HandleAsync_Success(bool withAction)
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateVerifiedIdClaimValidationEvent(fixture);

        using var cts = new CancellationTokenSource();

        var expectedResponse = new VerifiedIdClaimValidationResponse
        {
            Data = new VerifiedIdClaimValidationResponsePayload
            {
                Actions = withAction
                    ? new List<EntraAction>
                    {
                        new VerifiedIdClaimValidationPassAction()
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
            ? EntraOdataTypes.VerifiedIdClaimValidation.Pass
            : "None";

        logged.Should().Be(expected);

        _logger.Scopes.Should().ContainSingle();

        var scope = (TestScope)_logger.Scopes.Single();
        var dict = (Dictionary<string, object?>)scope.State;

        dict.Should().ContainKey("CorrelationId").WhoseValue.Should().Be(evt.CorrelationId);
        dict.Should().ContainKey("EventType").WhoseValue.Should().Be(evt.Type);
        dict.Should().ContainKey("EventName").WhoseValue.Should().Be(nameof(VerifiedIdClaimValidationEvent));
    }

    [Fact]
    public async Task HandleAsync_Fail()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateVerifiedIdClaimValidationEvent(fixture);

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
            .BeOfType<VerifiedIdClaimValidationFailedAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.Failed);
        action.FailedClaims.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_InvalidRequest()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestData.CreateVerifiedIdClaimValidationEvent(fixture, valid: false);

        // Act
        var response = await _sut.HandleAsync(evt, CancellationToken.None);

        // Assert
        _sut.CoreTest.HandleCoreCallCount.Should().Be(0);

        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<VerifiedIdClaimValidationFailedAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.Failed);
        action.FailedClaims.Should().BeEmpty();
    }
}
