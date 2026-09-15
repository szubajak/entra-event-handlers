using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.TestData;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.UnitTests.Utils.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Handlers.Base;

public class TokenIssuanceStartHandlerTests
{
    private readonly TestTokenIssuanceStartHandler _sut;

    private readonly Fixture _fixture = new();
    private readonly TestLogger _logger;

    public TokenIssuanceStartHandlerTests()
    {
        _logger = new TestLogger();

        _sut = new TestTokenIssuanceStartHandler(_logger);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task HandleAsync_Success(bool withAction)
    {
        // Arrange
        var evt = TestEvents.CreateTokenIssuanceStartEvent(_fixture);

        using var cts = new CancellationTokenSource();

        var expectedResponse = new TokenIssuanceStartResponse
        {
            Data = new TokenIssuanceStartResponsePayload
            {
                Actions = withAction
                    ? new List<EntraAction>
                    {
                        new ProvideClaimsForTokenAction()
                    }
                    : Array.Empty<EntraAction>()
            }
        };

        _sut.ResponseToReturn = expectedResponse;

        // Act
        var result = await _sut.HandleAsync(evt, cts.Token);

        // Assert
        result.Should().NotBeNull();

        var response = result.Response;
        response.Should().BeEquivalentTo(expectedResponse);

        _sut.CoreTest.HandleCoreCallCount.Should().Be(1);
        _sut.CoreTest.CapturedCancellationToken.Should().Be(cts.Token);

        _logger.Entries.Should().Contain(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("Starting Entra event handling."));

        var success = _logger.Entries.Single(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("Entra event handled successfully."));

        var state = success.State.As<IReadOnlyList<KeyValuePair<string, object>>>();
        var logged = state.Single(kv => kv.Key == "ActionType").Value?.ToString();

        var expected = withAction
            ? EntraOdataTypes.TokenIssuanceStart.ProvideClaimsForToken
            : "None";

        _logger.Scopes.Should().ContainSingle();

        var scope = (TestScope)_logger.Scopes.Single();
        var dict = (Dictionary<string, object?>)scope.State;

        dict.Should().ContainKey("CorrelationId").WhoseValue.Should().Be(evt.CorrelationId);
        dict.Should().ContainKey("EventType").WhoseValue.Should().Be(evt.Type);
        dict.Should().ContainKey("EventName").WhoseValue.Should().Be(nameof(TokenIssuanceStartEvent));
    }

    [Fact]
    public async Task HandleAsync_Fail()
    {
        // Arrange
        var evt = TestEvents.CreateTokenIssuanceStartEvent(_fixture);

        _sut.CoreTest.ShouldThrow = true;

        // Act
        var result = await _sut.HandleAsync(evt, CancellationToken.None);

        // Assert
        _sut.CoreTest.HandleCoreCallCount.Should().Be(1);

        _logger.Entries.Should().Contain(e =>
            e.Level == LogLevel.Error &&
            e.Message.Contains("Unexpected failure occurred during Entra event handling."));

        result.Should().NotBeNull();

        var response = result.Response;
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ProvideClaimsForTokenAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.TokenIssuanceStart.ProvideClaimsForToken);
        action.Claims.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_InvalidRequest()
    {
        // Arrange
        var evt = TestEvents.CreateTokenIssuanceStartEvent(_fixture, valid: false);

        // Act
        var result = await _sut.HandleAsync(evt, CancellationToken.None);

        // Assert
        _sut.CoreTest.HandleCoreCallCount.Should().Be(0);

        result.Should().NotBeNull();

        var response = result.Response;
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ProvideClaimsForTokenAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.TokenIssuanceStart.ProvideClaimsForToken);
        action.Claims.Should().BeEmpty();
    }
}
