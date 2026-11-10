using Entra.EventHandlers.Abstractions.Errors;
using FluentAssertions;

namespace Entra.EventHandlers.Abstractions.UnitTests.Errors;

public class EntraSecurityExceptionTests
{
    [Fact]
    public void Constructor_MessageAndInnerExceptionProvided_SetsProperties()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new EntraSecurityException("Security error", innerException);

        // Assert
        exception.Message.Should().Be("Security error");
        exception.InnerException.Should().Be(innerException);
    }
}
