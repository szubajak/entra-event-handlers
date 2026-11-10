using System.Text.Json;
using AutoFixture;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;
using FluentAssertions;
using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;

namespace Entra.EventHandlers.Abstractions.UnitTests.Responses;

public class PasswordSubmitResponseSerializationTests
{
    private readonly Fixture _fixture = new();
    private readonly JsonDiffPatch _jsonDiffPatch = new();

    private static string GetExpectedResponse(string odataType, string nonce) =>
        $$"""
            {
              "data": {
                "@odata.type": "microsoft.graph.onPasswordSubmitResponseData",
                "actions": [
                  {
                    "@odata.type": "{{odataType}}",
                  }
                ],
                "nonce": "{{nonce}}"
              }
            }
            """;

    [Fact]
    public void Serialize_MigratePasswordAction_ReturnsExpectedJson()
    {
        // Arrange
        var nonce = _fixture.Create<string>();

        var expectedJson = GetExpectedResponse("microsoft.graph.passwordSubmit.MigratePassword", nonce);

        var response = new PasswordSubmitResponse
        {
            Data = new PasswordSubmitResponsePayload
            {
                Actions = [new PasswordSubmitAction(PasswordSubmitActionType.MigratePassword)],
                Nonce = nonce,
            },
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }

    [Fact]
    public void Serialize_UpdatePasswordAction_ReturnsExpectedJson()
    {
        // Arrange
        var nonce = _fixture.Create<string>();

        var expectedJson = GetExpectedResponse("microsoft.graph.passwordSubmit.UpdatePassword", nonce);

        var response = new PasswordSubmitResponse
        {
            Data = new PasswordSubmitResponsePayload
            {
                Actions = [new PasswordSubmitAction(PasswordSubmitActionType.UpdatePassword)],
                Nonce = nonce,
            },
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }

    [Fact]
    public void Serialize_RetryAction_ReturnsExpectedJson()
    {
        // Arrange
        var nonce = _fixture.Create<string>();

        var expectedJson = GetExpectedResponse("microsoft.graph.passwordSubmit.Retry", nonce);

        var response = new PasswordSubmitResponse
        {
            Data = new PasswordSubmitResponsePayload
            {
                Actions = [new PasswordSubmitAction(PasswordSubmitActionType.Retry)],
                Nonce = nonce,
            },
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }

    [Fact]
    public void Serialize_BlockAction_ReturnsExpectedJson()
    {
        // Arrange
        var nonce = _fixture.Create<string>();

        var expectedJson = GetExpectedResponse("microsoft.graph.passwordSubmit.Block", nonce);

        var response = new PasswordSubmitResponse
        {
            Data = new PasswordSubmitResponsePayload
            {
                Actions = [new PasswordSubmitAction(PasswordSubmitActionType.Block)],
                Nonce = nonce,
            },
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        var diff = _jsonDiffPatch.Diff(JToken.Parse(json), JToken.Parse(expectedJson));
        diff.Should().BeNull();
    }
}
