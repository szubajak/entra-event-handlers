using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;

namespace Entra.EventHandlers.Abstractions.UnitTests.Results;

public sealed class EntraHandlerResultTests
{
    [Fact]
    public void HasException_ExceptionIsNull_ReturnsFalse()
    {
        // Arrange
        var sut = new EntraHandlerResult<TestResponse>(new TestResponse());

        // Act
        var result = sut.HasException;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasException_ExceptionIsProvided_ReturnsTrue()
    {
        // Arrange
        var sut = new EntraHandlerResult<TestResponse>(new TestResponse(), new InvalidOperationException());

        // Act
        var result = sut.HasException;

        // Assert
        result.Should().BeTrue();
    }
}
