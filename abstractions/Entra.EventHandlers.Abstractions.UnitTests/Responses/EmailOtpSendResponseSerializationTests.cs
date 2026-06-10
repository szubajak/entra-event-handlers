using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Entra.EventHandlers.Abstractions.UnitTests.Responses;

public class EmailOtpSendResponseSerializationTests
{
    [Fact]
    public void ContinueWithDefaultBehaviorAction_SerializesToExpectedJson()
    {
        // Arrange
        var expectedJson =
        """
        {
          "data": {
            "@odata.type": "microsoft.graph.OnOtpSendResponseData",
            "actions": [
              {
                "@odata.type": "microsoft.graph.OtpSend.continueWithDefaultBehavior"
              }
            ]
          }
        }
        """;

        var response = new EmailOtpSendResponse
        {
            Data = new EmailOtpSendResponsePayload
            {
                Actions = [new ContinueAction(ContinueActionType.EmailOtpSendContinueWithDefaultBehavior)]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);

        // Assert
        JToken.Parse(json).Should().BeEquivalentTo(JToken.Parse(expectedJson));
    }
}
