using System.Text.Json;
using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Responses;
using FluentAssertions;
using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;

namespace Entra.EventHandlers.Abstractions.UnitTests.Responses;

public class TokenIssuanceStartResponseSerializationTests
{
    private readonly JsonDiffPatch _jsonDiffPatch = new();

    [Fact]
    public void Serialize_ResponseWithCustomClaims_ReturnsExpectedJson()
    {
        // Arrange
        var fixture = new Fixture();

        var (claim1, val1) = (fixture.Create<string>(), fixture.Create<string>());
        var (claim2, val2) = (fixture.Create<string>(), fixture.CreateMany<string>());

        var expectedJson = $$"""
            {
              "data": {
                "@odata.type": "microsoft.graph.onTokenIssuanceStartResponseData",
                "actions": [
                  {
                    "@odata.type": "microsoft.graph.tokenIssuanceStart.provideClaimsForToken",
                    "claims": {
                      "{{claim1}}": "{{val1}}",
                      "{{claim2}}": {{JsonSerializer.Serialize(val2)}}
                    }
                  }
                ]
              }
            }
            """;

        var response = new TokenIssuanceStartResponse
        {
            Data = new TokenIssuanceStartResponsePayload
            {
                Actions = [new ProvideClaimsForTokenAction { Claims = { [claim1] = val1, [claim2] = val2 } }],
            },
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }

    [Fact]
    public void Serialize_ResponseWithoutAdditionalClaims_ReturnsExpectedJson()
    {
        // Arrange
        var expectedJson = $$"""
            {
              "data": {
                "@odata.type": "microsoft.graph.onTokenIssuanceStartResponseData",
                "actions": [
                  {
                    "@odata.type": "microsoft.graph.tokenIssuanceStart.provideClaimsForToken",
                    "claims": {
                    }
                  }
                ]
              }
            }
            """;

        var response = new TokenIssuanceStartResponse
        {
            Data = new TokenIssuanceStartResponsePayload { Actions = [new ProvideClaimsForTokenAction()] },
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }
}
