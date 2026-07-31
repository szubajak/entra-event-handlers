using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;
using FluentAssertions;
using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Entra.EventHandlers.Abstractions.UnitTests.Responses;

public class AttributeCollectionSubmitResponseSerializationTests
{
    private readonly JsonDiffPatch _jsonDiffPatch = new();

    [Fact]
    public void ContinueWithDefaultBehaviorAction_SerializesToExpectedJson()
    {
        // Arrange
        var expectedJson =
        """
        {
          "data": {
            "@odata.type": "microsoft.graph.onAttributeCollectionSubmitResponseData",
            "actions": [
              {
                "@odata.type": "microsoft.graph.attributeCollectionSubmit.continueWithDefaultBehavior"
              }
            ]
          }
        }
        """;

        var response = new AttributeCollectionSubmitResponse
        {
            Data = new AttributeCollectionSubmitResponsePayload
            {
                Actions = [new ContinueAction(ContinueActionType.AttributeCollectionSubmitContinueWithDefaultBehavior)]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }

    [Fact]
    public void ModifyAttributeValuesAction_SerializesToExpectedJson()
    {
        // Arrange
        var fixture = new Fixture();

        var (attr1, val1) = (fixture.Create<string>(), fixture.Create<string>());
        var (attr2, val2) = (fixture.Create<string>(), fixture.Create<bool>());

        var expectedJson =
        $$"""
        {
          "data": {
            "@odata.type": "microsoft.graph.onAttributeCollectionSubmitResponseData",
            "actions": [
              {
                "@odata.type": "microsoft.graph.attributeCollectionSubmit.modifyAttributeValues",
                "attributes": {
                  "{{attr1}}": "{{val1}}",
                  "{{attr2}}": {{val2.ToString().ToLowerInvariant()}}
                }
              }
            ]
          }
        }
        """;

        var response = new AttributeCollectionSubmitResponse
        {
            Data = new AttributeCollectionSubmitResponsePayload
            {
                Actions = 
                [
                    new ModifyAttributeValuesAction
                    {
                        Attributes =
                        {
                            [attr1] = val1,
                            [attr2] = val2
                        }
                    }
                ]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
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
            "@odata.type": "microsoft.graph.onAttributeCollectionSubmitResponseData",
            "actions": [
              {
                "@odata.type": "microsoft.graph.attributeCollectionSubmit.showBlockPage",
                "title": "{{title}}",
                "message": "{{message}}"
              }
            ]
          }
        }
        """;

        var response = new AttributeCollectionSubmitResponse
        {
            Data = new AttributeCollectionSubmitResponsePayload
            {
                Actions = 
                [
                    new ShowBlockPageAction(ShowBlockPageActionType.AttributeCollectionSubmitShowBlockPage)
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
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }

    [Fact]
    public void ShowValidationErrorAction_SerializesToExpectedJson()
    {
        // Arrange
        var fixture = new Fixture();

        var message = fixture.Create<string>();
        var (attr1, msg1) = (fixture.Create<string>(), fixture.Create<string>());
        var (attr2, msg2) = (fixture.Create<string>(), fixture.Create<string>());

        var expectedJson =
        $$"""
        {
          "data": {
            "@odata.type": "microsoft.graph.onAttributeCollectionSubmitResponseData",
            "actions": [
              {
                "@odata.type": "microsoft.graph.attributeCollectionSubmit.showValidationError",
                "message": "{{message}}",
                "attributeErrors": {
                  "{{attr1}}": "{{msg1}}",
                  "{{attr2}}": "{{msg2}}",
                }
              }
            ]
          }
        }
        """;

        var response = new AttributeCollectionSubmitResponse
        {
            Data = new AttributeCollectionSubmitResponsePayload
            {
                Actions =
                [
                    new ShowValidationErrorAction
                    {
                        Message = message,
                        AttributeErrors =
                        {
                            [attr1] = msg1,
                            [attr2] = msg2
                        }
                    }
                ]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }
}
