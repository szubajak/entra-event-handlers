using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Builders.ResponseBuilders;
using FluentAssertions;

namespace Entra.EventHandlers.UnitTests.Builders.ResponseBuilders;

public class AttributeCollectionStartResponseBuilderTests
{
    private readonly AttributeCollectionStartResponseBuilder _sut;

    private readonly Fixture _fixture = new();

    public AttributeCollectionStartResponseBuilderTests()
    {
        _sut = new AttributeCollectionStartResponseBuilder();
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
        response.Data.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.ResponseData);

        var action = response.Data.Actions.Should().ContainSingle().Subject;
        action.Should().BeOfType<ContinueAction>();
        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.ContinueWithDefaultBehavior);
    }

    [Fact]
    public void Build_ReturnsResponseWith_PrefillValuesAction()
    {
        // Arrange
        var (input1, val1) = (_fixture.Create<string>(), _fixture.Create<string>());
        var (input2, val2) = (_fixture.Create<string>(), _fixture.Create<bool>());

        var inputs = new Dictionary<string, object>
        {
            [input1] = val1,
            [input2] = val2
        };

        // Act
        var response = _sut
            .SetPrefillValues(inputs)
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.ResponseData);
        response.Data.Actions.Should().HaveCount(1);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<SetPrefillValuesAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.SetPrefillValues);
        action.Inputs.Should().BeEquivalentTo(inputs);
    }

    [Fact]
    public void Build_ReturnsResponseWith_PrefillValuesAction_UsingFluentBuilder()
    {
        // Arrange
        var (input1, val1) = (_fixture.Create<string>(), _fixture.Create<string>());
        var (input2, val2) = (_fixture.Create<string>(), _fixture.Create<bool>());

        // Act
        var response = _sut
            .SetPrefillValues()
                .Add(input1, val1)
                .Add(input2, val2)
            .Done()
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.ResponseData);
        response.Data.Actions.Should().HaveCount(1);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<SetPrefillValuesAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.SetPrefillValues);

        action.Inputs.Should().BeEquivalentTo(new Dictionary<string, object>
        {
            [input1] = val1,
            [input2] = val2
        });
    }

    [Fact]
    public void Build_ReturnsResponseWith_ShowBlockPageAction()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var message = _fixture.Create<string>();

        // Act
        var response = _sut
            .ShowBlockPage(title, message)
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.ResponseData);
        response.Data.Actions.Should().HaveCount(1);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ShowBlockPageAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.ShowBlockPage);
        action.Title.Should().Be(title);
        action.Message.Should().Be(message);
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
