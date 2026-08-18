using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Workforce.Builders.ResponseBuilders;
using FluentAssertions;

namespace Entra.EventHandlers.Workforce.UnitTests.Builders.ResponseBuilders;

public class VerifiedIdClaimValidationResponseBuilderTests
{
    private readonly VerifiedIdClaimValidationResponseBuilder _sut;

    private readonly Fixture _fixture = new();

    public VerifiedIdClaimValidationResponseBuilderTests()
    {
        _sut = new VerifiedIdClaimValidationResponseBuilder();
    }

    [Fact]
    public void Build_ReturnsResponseWith_PassAction()
    {
        // Act
        var response = _sut
            .Pass()
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.ResponseData);

        var action = response.Data.Actions.Should().ContainSingle().Subject;
        action.Should().BeOfType<VerifiedIdClaimValidationPassAction>();
        action.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.Pass);
    }

    [Fact]
    public void Build_ReturnsResponseWith_FailedAction()
    {
        // Arrange
        var failedClaims = _fixture.CreateMany<string>(3).ToList();

        // Act
        var response = _sut
            .Failed(failedClaims)
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.ResponseData);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<VerifiedIdClaimValidationFailedAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.Failed);
        action.FailedClaims.Should().BeEquivalentTo(failedClaims);
    }

    [Fact]
    public void Build_ReturnsResponseWith_FailedAction_UsingFluentBuilder()
    {
        // Arrange
        var claim1 = _fixture.Create<string>();
        var claim2 = _fixture.Create<string>();

        // Act
        var response = _sut
            .Failed()
                .Add(claim1)
                .Add(claim2)
            .Done()
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.ResponseData);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<VerifiedIdClaimValidationFailedAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.Failed);

        action.FailedClaims.Should().BeEquivalentTo([claim1, claim2]);
    }

    [Fact]
    public void Build_ThrowsInvalidOperationException_WhenNoActionWasSelected()
    {
        // Act
        Action act = () => _sut.Build();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("An action must be selected before building the response.");
    }
}
