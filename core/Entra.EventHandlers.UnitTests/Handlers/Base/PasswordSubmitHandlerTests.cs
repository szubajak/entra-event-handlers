using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Interfaces;
using Entra.EventHandlers.Protocol.PasswordSubmit;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.UnitTests.Utils;
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
    private readonly IPasswordContextCryptoService _cryptoService;

    public PasswordSubmitHandlerTests()
    {
        _logger = new TestLogger();
        _cryptoService = Substitute.For<IPasswordContextCryptoService>();

        _sut = new TestPasswordSubmitHandler(_logger, _cryptoService);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task HandleAsync_Success(bool withAction)
    {
        // Arrange
        var evt = TestData.CreatePasswordSubmitEvent(_fixture);

        var decrypted = _fixture.Create<DecryptedPasswordContext>();
        _cryptoService.Decrypt(evt.Data.EncryptedPasswordContext)
            .Returns(decrypted);

        using var cts = new CancellationTokenSource();

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
        var response = await _sut.HandleAsync(evt, cts.Token);

        // Assert
        response.Should().Be(expectedResponse);
        _sut.PassedDecryptedPasswordContext.Should().Be(decrypted);

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
        var evt = TestData.CreatePasswordSubmitEvent(_fixture);

        var decrypted = _fixture.Create<DecryptedPasswordContext>();
        _cryptoService.Decrypt(evt.Data.EncryptedPasswordContext)
               .Returns(decrypted);

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
        var evt = TestData.CreatePasswordSubmitEvent(_fixture, valid: false);

        // Act
        Func<Task> act = () => _sut.HandleAsync(evt, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
