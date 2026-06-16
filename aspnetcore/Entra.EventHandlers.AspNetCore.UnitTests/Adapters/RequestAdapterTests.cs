using Entra.EventHandlers.Abstractions.Errors;
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
    public async Task ReadEvent_WhenBodyIsEmpty_ThrowsDeserializationException()
    {
        // Arrange
        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(""));

        // Act
        var act = async () => await _sut.ReadEvent<TestEvent>(ctx);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Request body is empty.");
    }

    [Fact]
    public async Task ReadEvent_WhenJsonIsInvalid_ThrowsInvalidJsonException()
    {
        // Arrange
        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{ invalid json "));

        // Act
        var act = async () => await _sut.ReadEvent<TestEvent>(ctx);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Invalid JSON payload.");
    }

    [Fact]
    public async Task ReadEvent_WhenJsonDeserializesToNull_ThrowsUnableToDeserialize()
    {
        // Arrange
        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("null"));

        // Act
        var act = async () => await _sut.ReadEvent<TestEvent>(ctx);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Unable to deserialize event.");
    }

    [Fact]
    public async Task ReadEvent_WhenUnexpectedExceptionOccurs_ThrowsFailedToDeserialize()
    {
        // Arrange
        var ctx = new DefaultHttpContext();
        ctx.Request.Body = new ThrowingStream();

        // Act
        var act = async () => await _sut.ReadEvent<TestEvent>(ctx);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Failed to deserialize event.");
    }
}
