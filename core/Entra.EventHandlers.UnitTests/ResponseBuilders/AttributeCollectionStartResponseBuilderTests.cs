using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.ResponseBuilders;
using FluentAssertions;

namespace Entra.EventHandlers.UnitTests.ResponseBuilders;

public class AttributeCollectionStartResponseBuilderTests
{
    private readonly AttributeCollectionStartResponseBuilder _sut;

    public AttributeCollectionStartResponseBuilderTests()
    {
        _sut = new AttributeCollectionStartResponseBuilder();
    }

    [Fact]
    public void ContinueWithDefaultBehavior_Build_Success()
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
    public void SetPrefillValues_Build_Success()
    {
        // Arrange
        var inputs = new Dictionary<string, object>
        {
            { "key1", "value1,value2,value3" },
            { "key2", true }
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
    public void ShowBlockPage_Build_Success()
    {
        // Arrange
        var title = "Hold tight...";
        var message = "Your access request is already processing. You'll be notified when your request has been approved.";

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
}
