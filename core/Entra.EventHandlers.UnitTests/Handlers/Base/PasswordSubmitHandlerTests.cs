using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.TestData;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.UnitTests.Utils.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Entra.EventHandlers.UnitTests.Handlers.Base;

public class PasswordSubmitHandlerTests
{
    private readonly TestPasswordSubmitHandler _sut;

    private readonly Fixture _fixture = new();
    private readonly TestLogger _logger;
    private readonly IPasswordContextDecryptor _decryptor;

    public PasswordSubmitHandlerTests()
    {
        _logger = new TestLogger();
        _decryptor = Substitute.For<IPasswordContextDecryptor>();

        _sut = new TestPasswordSubmitHandler(_logger, _decryptor);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task HandleAsync_Success(bool withAction)
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;

        var evt = TestEvents.CreatePasswordSubmitEvent(_fixture);

        var decrypted = _fixture.Create<DecryptedPasswordContext>();
        _decryptor.DecryptAsync(evt.Data.EncryptedPasswordContext, ct)
            .Returns(decrypted);

        var expectedResponse = new PasswordSubmitResponse
        {
            Data = new PasswordSubmitResponsePayload
            {
                Actions = withAction
                    ? new List<EntraAction>
                    {
                        new PasswordSubmitAction(PasswordSubmitActionType.MigratePassword)
                    }
                    : Array.Empty<EntraAction>(),
                Nonce = "some-nonce"
            }
        };

        _sut.ResponseToReturn = expectedResponse;

        // Act
        var result = await _sut.HandleAsync(evt, ct);

        // Assert
        result.Should().NotBeNull();

        var response = result.Response;
        response.Should().BeEquivalentTo(expectedResponse);

        _sut.PassedDecryptedPasswordContext.Should().Be(decrypted);

        _sut.CoreTest.HandleCoreCallCount.Should().Be(1);
        _sut.CoreTest.CapturedCancellationToken.Should().Be(ct);

        _logger.Entries.Should().Contain(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("Starting Entra event handling."));

        var success = _logger.Entries.Single(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("Entra event handled successfully."));

        var state = success.State.As<IReadOnlyList<KeyValuePair<string, object>>>();
        var logged = state.Single(kv => kv.Key == "ActionType").Value?.ToString();

        var expected = withAction
            ? EntraOdataTypes.PasswordSubmit.MigratePassword
            : "None";

        logged.Should().Be(expected);

        _logger.Scopes.Should().ContainSingle();

        var scope = (TestScope)_logger.Scopes.Single();
        var dict = (Dictionary<string, object?>)scope.State;

        dict.Should().ContainKey("CorrelationId").WhoseValue.Should().Be(evt.CorrelationId);
        dict.Should().ContainKey("EventType").WhoseValue.Should().Be(evt.Type);
        dict.Should().ContainKey("EventName").WhoseValue.Should().Be(nameof(PasswordSubmitEvent));
    }

    [Fact]
    public async Task HandleAsync_Fail()
    {
        // Arrange
        var evt = TestEvents.CreatePasswordSubmitEvent(_fixture);

        var decrypted = _fixture.Create<DecryptedPasswordContext>();
        _decryptor.DecryptAsync(evt.Data.EncryptedPasswordContext, Arg.Any<CancellationToken>())
               .Returns(decrypted);

        _sut.CoreTest.ShouldThrow = true;

        // Act
        var result = await _sut.HandleAsync(evt, TestContext.Current.CancellationToken);

        // Assert
        _sut.CoreTest.HandleCoreCallCount.Should().Be(1);

        _logger.Entries.Should().Contain(e =>
            e.Level == LogLevel.Error &&
            e.Message.Contains("Unexpected failure occurred during Entra event handling."));

        result.Should().NotBeNull();

        var response = result.Response;
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.Nonce.Should().Be(decrypted.Nonce);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<PasswordSubmitAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.Block);
    }

    [Fact]
    public async Task HandleAsync_InvalidRequest()
    {
        // Arrange
        var evt = TestEvents.CreatePasswordSubmitEvent(_fixture, valid: false);

        // Act
        Func<Task> act = () => _sut.HandleAsync(evt, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
