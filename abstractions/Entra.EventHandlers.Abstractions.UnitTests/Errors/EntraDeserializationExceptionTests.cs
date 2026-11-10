using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using FluentAssertions;

namespace Entra.EventHandlers.Abstractions.UnitTests.Errors;

public class EntraDeserializationExceptionTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Constructor_MessageProvided_SetsMessage()
    {
        // Arrange
        var message = _fixture.Create<string>();

        // Act
        var ex = new EntraDeserializationException(message);

        // Assert
        ex.Message.Should().Be(message);
        ex.InnerException.Should().BeNull();
    }

    [Fact]
    public void Constructor_MessageAndInnerExceptionProvided_SetsProperties()
    {
        // Arrange
        var message = _fixture.Create<string>();
        var inner = new InvalidOperationException(_fixture.Create<string>());

        // Act
        var ex = new EntraDeserializationException(message, inner);

        // Assert
        ex.Message.Should().Be(message);
        ex.InnerException.Should().Be(inner);
    }
}
