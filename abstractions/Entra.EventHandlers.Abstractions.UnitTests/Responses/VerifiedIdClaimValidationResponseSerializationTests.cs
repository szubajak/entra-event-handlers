using System.Text.Json;
using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Responses;
using FluentAssertions;
using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;

namespace Entra.EventHandlers.Abstractions.UnitTests.Responses;

public class VerifiedIdClaimValidationResponseSerializationTests
{
    private readonly JsonDiffPatch _jsonDiffPatch = new();

    [Fact]
    public void Serialize_PassAction_ReturnsExpectedJson()
    {
        // Arrange
        var expectedJson = $$"""
            {
              "data": {
                "@odata.type": "microsoft.graph.onVerifiedIdClaimValidationResponseData",
                "actions": [
                  {
                    "@odata.type": "microsoft.graph.verifiedIdClaimValidation.pass"
                  }
                ]
              }
            }
            """;

        var response = new VerifiedIdClaimValidationResponse
        {
            Data = new VerifiedIdClaimValidationResponsePayload { Actions = [new VerifiedIdClaimValidationPassAction()] },
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }

    [Fact]
    public void Serialize_FailedAction_ReturnsExpectedJson()
    {
        // Arrange
        var fixture = new Fixture();

        var claim1 = fixture.Create<string>();
        var claim2 = fixture.Create<string>();

        var expectedJson = $$"""
            {
              "data": {
                "@odata.type": "microsoft.graph.onVerifiedIdClaimValidationResponseData",
                "actions": [
                  {
                    "@odata.type": "microsoft.graph.verifiedIdClaimValidation.failed",
                    "failedClaims": [
                        "{{claim1}}",
                        "{{claim2}}"
                    ]
                  }
                ]
              }
            }
            """;

        var response = new VerifiedIdClaimValidationResponse
        {
            Data = new VerifiedIdClaimValidationResponsePayload
            {
                Actions = [new VerifiedIdClaimValidationFailedAction { FailedClaims = [claim1, claim2] }],
            },
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }
}
