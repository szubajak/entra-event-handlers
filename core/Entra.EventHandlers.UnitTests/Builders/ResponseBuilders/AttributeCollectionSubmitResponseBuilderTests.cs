using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Builders.ResponseBuilders;
using FluentAssertions;

namespace Entra.EventHandlers.UnitTests.Builders.ResponseBuilders;

public class AttributeCollectionSubmitResponseBuilderTests
{
    private readonly AttributeCollectionSubmitResponseBuilder _sut;

    public AttributeCollectionSubmitResponseBuilderTests()
    {
        _sut = new AttributeCollectionSubmitResponseBuilder();
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
        response.Data.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ResponseData);

        var action = response.Data.Actions.Should().ContainSingle().Subject;
        action.Should().BeOfType<ContinueAction>();
        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ContinueWithDefaultBehavior);
    }

    [Fact]
    public void Build_ReturnsResponseWith_ModifyAttributeValuesAction()
    {
        // Arrange
        var fixture = new Fixture();

        var (attr1, val1) = (fixture.Create<string>(), fixture.Create<string>());
        var (attr2, val2) = (fixture.Create<string>(), fixture.Create<bool>());

        var attributes = new Dictionary<string, object>
        {
            [attr1] = val1,
            [attr2] = val2
        };

        // Act
        var response = _sut
            .ModifyAttributeValues(attributes)
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ResponseData);
        response.Data.Actions.Should().HaveCount(1);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ModifyAttributeValuesAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ModifyAttributeValues);
        action.Attributes.Should().BeEquivalentTo(attributes);
    }

    [Fact]
    public void Build_ReturnsResponseWith_ShowBlockPageAction()
    {
        // Arrange
        var fixture = new Fixture();

        var title = fixture.Create<string>();
        var message = fixture.Create<string>();

        // Act
        var response = _sut
            .ShowBlockPage(title, message)
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ResponseData);
        response.Data.Actions.Should().HaveCount(1);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ShowBlockPageAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ShowBlockPage);
        action.Title.Should().Be(title);
        action.Message.Should().Be(message);
    }

    [Fact]
    public void Build_ReturnsResponseWith_ShowValidationErrorAction()
    {
        // Arrange
        var fixture = new Fixture();
        var message = fixture.Create<string>();

        var (attr1, msg1) = (fixture.Create<string>(), fixture.Create<string>());
        var (attr2, msg2) = (fixture.Create<string>(), fixture.Create<string>());

        var attributeErrors = new Dictionary<string, string>
        {
            [attr1] = msg1,
            [attr2] = msg2
        };

        // Act
        var response = _sut
            .ShowValidationError(message, attributeErrors)
            .Build();

        // Assert
        response.Should().NotBeNull();
        response.Data.Should().NotBeNull();
        response.Data.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ResponseData);
        response.Data.Actions.Should().HaveCount(1);

        var action = response.Data.Actions
            .Single()
            .Should()
            .BeOfType<ShowValidationErrorAction>()
            .Subject;

        action.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionSubmit.ShowValidationError);

        action.Message.Should().Be(message);
        action.AttributeErrors.Should().BeEquivalentTo(attributeErrors);
    }
}
