using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Adapters;

public class RequestAdapterTests
{
    private readonly RequestAdapter _sut;

    public RequestAdapterTests()
    {
        _sut = new RequestAdapter();
    }

    [Fact]
    public async Task ReadEventAsync_WhenBodyIsEmpty_ThrowsDeserializationException()
    {
        // Arrange
        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(""));

        // Act
        var act = async () => await _sut.ReadEventAsync<TestEvent>(ctx);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Request body is empty.");
    }

    [Fact]
    public async Task ReadEventAsync_WhenJsonIsInvalid_ThrowsInvalidJsonException()
    {
        // Arrange
        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{ invalid json "));

        // Act
        var act = async () => await _sut.ReadEventAsync<TestEvent>(ctx);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Invalid JSON payload.");
    }

    [Fact]
    public async Task ReadEventAsync_WhenJsonDeserializesToNull_ThrowsUnableToDeserialize()
    {
        // Arrange
        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("null"));

        // Act
        var act = async () => await _sut.ReadEventAsync<TestEvent>(ctx);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Unable to deserialize event.");
    }

    [Fact]
    public async Task ReadEventAsync_WhenUnexpectedExceptionOccurs_ThrowsFailedToDeserialize()
    {
        // Arrange
        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new ThrowingStream();

        // Act
        var act = async () => await _sut.ReadEventAsync<TestEvent>(ctx);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Failed to deserialize event.");
    }

    [Fact]
    public async Task ReadEventAsync_WhenJsonIsValid_ReturnsDeserializedEvent()
    {
        // Arrange
        var expectedResult = new TestEvent();

        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

        // Act
        var result = await _sut.ReadEventAsync<TestEvent>(ctx);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task ReadEventAsync_NonGeneric_Calls_Generic_With_EntraEvent()
    {
        // Arrange
        var json =
        """
        { 
          "type": "microsoft.graph.authenticationEvent.attributeCollectionStart",
          "source": "source",
          "data": {
            "authenticationContext": {
              "correlationId": "00000000-0000-0000-0000-000000000000"
            }
          }
        }
        """;

        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(json));

        // Act
        var result = await _sut.ReadEventAsync(ctx);

        // Assert
        result.Should().BeAssignableTo<EntraEvent>();
    }
}
