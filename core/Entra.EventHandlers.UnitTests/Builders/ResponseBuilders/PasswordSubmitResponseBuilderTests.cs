using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Builders.ResponseBuilders;
using FluentAssertions;

namespace Entra.EventHandlers.UnitTests.Builders.ResponseBuilders;

public class PasswordSubmitResponseBuilderTests
{
    private readonly PasswordSubmitResponseBuilder _sut;

    public PasswordSubmitResponseBuilderTests()
    {
        _sut = new PasswordSubmitResponseBuilder();
    }

    [Fact]
    public void Build_ReturnsResponseWith_MigratePassword()
    {
        // Act
        var fixture = new Fixture();
        var nonce = fixture.Create<string>();

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
        var fixture = new Fixture();
        var nonce = fixture.Create<string>();

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
        var fixture = new Fixture();
        var nonce = fixture.Create<string>();

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
        var fixture = new Fixture();
        var nonce = fixture.Create<string>();

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
}
