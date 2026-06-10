using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Entra.EventHandlers.Abstractions.UnitTests.Responses;

public class AttributeCollectionStartResponseSerializationTests
{
    [Fact]
    public void ContinueWithDefaultBehaviorAction_SerializesToExpectedJson()
    {
        // Arrange
        var expectedJson =
        """
        {
          "data": {
            "@odata.type": "microsoft.graph.onAttributeCollectionStartResponseData",
            "actions": [
              {
                "@odata.type": "microsoft.graph.attributeCollectionStart.continueWithDefaultBehavior"
              }
            ]
          }
        }
        """;

        var response = new AttributeCollectionStartResponse
        {
            Data = new AttributeCollectionStartResponsePayload
            {
                Actions = [new ContinueAction(ContinueActionType.AttributeCollectionStartContinueWithDefaultBehavior)]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        JToken.Parse(json).Should().BeEquivalentTo(JToken.Parse(expectedJson));
    }

    [Fact]
    public void SetPrefillValuesAction_SerializesToExpectedJson()
    {
        // Arrange
        var fixture = new Fixture();

        var (input1, val1) = (fixture.Create<string>(), fixture.Create<string>());
        var (input2, val2) = (fixture.Create<string>(), fixture.Create<bool>());

        var expectedJson =
        $$"""
        {
          "data": {
            "@odata.type": "microsoft.graph.onAttributeCollectionStartResponseData",
            "actions": [
              {
                "@odata.type": "microsoft.graph.attributeCollectionStart.setPrefillValues",
                "inputs": {
                  "{{input1}}": "{{val1}}",
                  "{{input2}}": {{val2.ToString().ToLowerInvariant()}}
                }
              }
            ]
          }
        }
        """;

        var response = new AttributeCollectionStartResponse
        {
            Data = new AttributeCollectionStartResponsePayload
            {
                Actions = 
                [
                    new SetPrefillValuesAction
                    {
                        Inputs =
                        {
                            [input1] = val1,
                            [input2] = val2
                        }
                    }
                ]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        JToken.Parse(json).Should().BeEquivalentTo(JToken.Parse(expectedJson));
    }

    [Fact]
    public void ShowBlockPageAction_SerializesToExpectedJson()
    {
        // Arrange
        var fixture = new Fixture();

        var title = fixture.Create<string>();
        var message = fixture.Create<string>();

        var expectedJson =
        $$"""
        {
          "data": {
            "@odata.type": "microsoft.graph.onAttributeCollectionStartResponseData",
            "actions": [
              {
                "@odata.type": "microsoft.graph.attributeCollectionStart.showBlockPage",
                "title": "{{title}}",
                "message": "{{message}}"
              }
            ]
          }
        }
        """;

        var response = new AttributeCollectionStartResponse
        {
            Data = new AttributeCollectionStartResponsePayload
            {
                Actions = 
                [
                    new ShowBlockPageAction(ShowBlockPageActionType.AttributeCollectionStartShowBlockPage)
                    {
                        Title = title,
                        Message = message
                    }
                ]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        JToken.Parse(json).Should().BeEquivalentTo(JToken.Parse(expectedJson));
    }
}
