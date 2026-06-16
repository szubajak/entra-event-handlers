using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Hosting.Extensions;
using FluentAssertions;

namespace Entra.EventHandlers.Hosting.UnitTests.Extensions;

public class ExceptionExtensionsTests
{
    [Theory]
    [MemberData(nameof(ExceptionTypes))]
    public void IsEntraException_Success(Type exceptionType, bool isEntraException)
    {
        // Arrange
        var exception = CreateException(exceptionType);

        // Act
        var result = exception.IsEntraException();

        // Assert
        result.Should().Be(isEntraException);
    }

    [Theory]
    [MemberData(nameof(ExceptionErrorCodes))]
    public void ToEntraErrorCode_Success(Type exceptionType, string errorCode)
    {
        // Arrange
        var exception = CreateException(exceptionType);

        // Act
        var result = exception.ToEntraErrorCode();

        // Assert
        result.Should().Be(errorCode);
    }

    public static TheoryData<Type, bool> ExceptionTypes() =>
        new()
        {
            { typeof(EntraDeserializationException), true },
            { typeof(EntraValidationException), true },
            { typeof(EntraHandlerNotFoundException), true },
            { typeof(InvalidOperationException), false }
        };

    public static TheoryData<Type, string> ExceptionErrorCodes() =>
    new()
    {
            { typeof(EntraDeserializationException), EntraErrorCodes.DeserializationError },
            { typeof(EntraValidationException), EntraErrorCodes.ValidationError },
            { typeof(EntraHandlerNotFoundException), EntraErrorCodes.HandlerNotFound },
            { typeof(InvalidOperationException), EntraErrorCodes.UnhandledException }
    };

    private static Exception CreateException(Type exceptionType) =>
    (exceptionType ?? throw new ArgumentNullException(nameof(exceptionType))) switch
    {
        var t when t.GetConstructor([typeof(Type)]) is not null
            => (Exception)t.GetConstructor([typeof(Type)])!.Invoke([typeof(object)]),

        var t when t.GetConstructor([typeof(string)]) is not null
            => (Exception)t.GetConstructor([typeof(string)])!.Invoke(["test message"]),

        var t => throw new InvalidOperationException($"Cannot create exception of type {t.Name}")
    };
}
