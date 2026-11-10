using System.Text.Json;
using Entra.EventHandlers.Abstractions.Events;
using FluentAssertions;

namespace Entra.EventHandlers.Abstractions.UnitTests.Events;

public class EntraEventDeserializationTests
{
    [Theory]
    [InlineData("microsoft.graph.authenticationEvent.unknownEvent")]
    [InlineData("MICROSOFT.GRAPH.AUTHENTICATIONEVENT.ATTRIBUTECOLLECTIONSTART")]
    public void Deserialize_UnknownEventType_ThrowsJsonException(string type)
    {
        // Arrange
        var json = $$"""
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
    public void Deserialize_MissingType_ThrowsNotSupportedException()
    {
        // Arrange
        var json = """
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
