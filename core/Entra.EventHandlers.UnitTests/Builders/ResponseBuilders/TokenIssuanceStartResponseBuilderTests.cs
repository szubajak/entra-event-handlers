using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Builders.ResponseBuilders;
using FluentAssertions;

namespace Entra.EventHandlers.UnitTests.Builders.ResponseBuilders;

public class TokenIssuanceStartResponseBuilderTests
{
    private readonly TokenIssuanceStartResponseBuilder _sut;

    public TokenIssuanceStartResponseBuilderTests()
    {
        _sut = new TokenIssuanceStartResponseBuilder();
    }

    [Fact]
    public void Build_ReturnsResponseWith_ProvideClaimsForTokenAction()
    {
        // Arrange
        var fixture = new Fixture();

        var (claim1, val1) = (fixture.Create<string>(), fixture.Create<string>());
        var (claim2, val2) = (fixture.Create<string>(), fixture.CreateMany<string>());

        var claims = new Dictionary<string, object>
        {
            [claim1] = val1,
            [claim2] = val2
        };

        // Act
        var response = _sut
            .ProvideClaimsForToken(claims)
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.TokenIssuanceStart.ResponseData);
        response.Data.Actions.Should().HaveCount(1);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ProvideClaimsForTokenAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.TokenIssuanceStart.ProvideClaimsForToken);
        action.Claims.Should().BeEquivalentTo(claims);
    }

    [Fact]
    public void Build_WithEmptyClaims_ReturnsActionWithEmptyClaimsObject()
    {
        // Arrange
        var claims = new Dictionary<string, object>();

        // Act
        var response = _sut
            .ProvideClaimsForToken(claims)
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ProvideClaimsForTokenAction>()
            .Subject;

        action.Claims.Should().BeEmpty();
    }
}
