using Entra.EventHandlers.Abstractions.Events;
using FluentAssertions;
using System.Text.Json;

namespace Entra.EventHandlers.Abstractions.UnitTests.Events;

public class EntraEventDeserializationTests
{
    [Theory]
    [InlineData("microsoft.graph.authenticationEvent.unknownEvent")]
    [InlineData("MICROSOFT.GRAPH.AUTHENTICATIONEVENT.ATTRIBUTECOLLECTIONSTART")]
    public void Deserialization_Unknown_EventType_Throws(string type)
    {
        // Arrange
        var json = 
        $$"""
        {
            "type": "{{type}}",
            "source": "/tenants/t/applications/a",
            "data": {}
        }
        """;

        // Act
        var act = () => JsonSerializer.Deserialize<EntraEvent>(json);

        // Assert
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Deserialization_Missing_Type_Throws()
    {
        // Arrange
        var json =
        """
        {
            "source": "/tenants/t/applications/a",
            "data": {}
        }
        """;

        // Act
        var act = () => JsonSerializer.Deserialize<EntraEvent>(json);

        // Assert
        act.Should().Throw<NotSupportedException>();
    }
}
