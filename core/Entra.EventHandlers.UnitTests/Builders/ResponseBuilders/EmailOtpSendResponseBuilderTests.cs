using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Builders.ResponseBuilders;
using FluentAssertions;

namespace Entra.EventHandlers.UnitTests.Builders.ResponseBuilders;

public class EmailOtpSendResponseBuilderTests
{
    private readonly EmailOtpSendResponseBuilder _sut;

    public EmailOtpSendResponseBuilderTests()
    {
        _sut = new EmailOtpSendResponseBuilder();
    }

    [Fact]
    public void Build_ReturnsResponseWith_ContinueWithDefaultBehaviorAction()
    {
        // Act
        var response = _sut
            .ContinueWithDefaultBehavior()
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.EmailOtpSend.ResponseData);

        var action = response.Data.Actions.Should().ContainSingle().Subject;
        action.Should().BeOfType<ContinueAction>();
        action.OdataType.Should().Be(EntraOdataTypes.EmailOtpSend.ContinueWithDefaultBehavior);
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
