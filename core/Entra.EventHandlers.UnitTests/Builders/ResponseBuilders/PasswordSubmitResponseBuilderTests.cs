using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Builders.ResponseBuilders;
using FluentAssertions;

namespace Entra.EventHandlers.UnitTests.Builders.ResponseBuilders;

public class PasswordSubmitResponseBuilderTests
{
    private readonly PasswordSubmitResponseBuilder _sut;

    private readonly Fixture _fixture = new();

    public PasswordSubmitResponseBuilderTests()
    {
        _sut = new PasswordSubmitResponseBuilder();
    }

    [Fact]
    public void Build_ReturnsResponseWith_MigratePassword()
    {
        // Act
        var nonce = _fixture.Create<string>();

        var response = _sut
            .WithNonce(nonce)
            .MigratePassword()
            .Build();

        // Assert
        response.Should().NotBeNull();

        var payload = response.Data;
        payload.Should().NotBeNull();
        payload.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.ResponseData);
        payload.Nonce.Should().Be(nonce);

        var action = payload.Actions.Should().ContainSingle().Subject;
        action.Should().BeOfType<PasswordSubmitAction>();
        action.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.MigratePassword);
    }

    [Fact]
    public void Build_ReturnsResponseWith_UpdatePassword()
    {
        // Act
        var nonce = _fixture.Create<string>();

        var response = _sut
            .WithNonce(nonce)
            .UpdatePassword()
            .Build();

        // Assert
        response.Should().NotBeNull();

        var payload = response.Data;
        payload.Should().NotBeNull();
        payload.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.ResponseData);
        payload.Nonce.Should().Be(nonce);

        var action = payload.Actions.Should().ContainSingle().Subject;
        action.Should().BeOfType<PasswordSubmitAction>();
        action.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.UpdatePassword);
    }

    [Fact]
    public void Build_ReturnsResponseWith_Retry()
    {
        // Act
        var nonce = _fixture.Create<string>();

        var response = _sut
            .WithNonce(nonce)
            .Retry()
            .Build();

        // Assert
        response.Should().NotBeNull();

        var payload = response.Data;
        payload.Should().NotBeNull();
        payload.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.ResponseData);
        payload.Nonce.Should().Be(nonce);

        var action = payload.Actions.Should().ContainSingle().Subject;
        action.Should().BeOfType<PasswordSubmitAction>();
        action.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.Retry);
    }

    [Fact]
    public void Build_ReturnsResponseWith_Block()
    {
        // Act
        var nonce = _fixture.Create<string>();

        var response = _sut
            .WithNonce(nonce)
            .Block()
            .Build();

        // Assert
        response.Should().NotBeNull();

        var payload = response.Data;
        payload.Should().NotBeNull();
        payload.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.ResponseData);
        payload.Nonce.Should().Be(nonce);

        var action = payload.Actions.Should().ContainSingle().Subject;
        action.Should().BeOfType<PasswordSubmitAction>();
        action.OdataType.Should().Be(EntraOdataTypes.PasswordSubmit.Block);
    }

    [Fact]
    public void Build_ThrowsInvalidOperationException_WhenNoActionWasSelected()
    {
        // Arrange
        _sut.WithNonce("test-nonce");
    
        // Act
        Action act = () => _sut.Build();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("An action must be selected before building the response.");
    }

    [Fact]
    public void Build_ThrowsInvalidOperationException_WhenNonceIsMissing()
    {
        // Act
        Action act = () => _sut.Build();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Nonce must be set before building the response.");
    }
}
