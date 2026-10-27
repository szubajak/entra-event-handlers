using System.Text;
using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Errors;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Adapters;

public class HttpAdapterTests
{
    private readonly HttpAdapter _sut;

    private readonly Fixture _fixture = new();

    public HttpAdapterTests()
    {
        _sut = new HttpAdapter();
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
        await act.Should().ThrowAsync<EntraDeserializationException>().WithMessage("Request body is empty.");
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
        await act.Should().ThrowAsync<EntraDeserializationException>().WithMessage("Invalid JSON payload.");
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
        await act.Should().ThrowAsync<EntraDeserializationException>().WithMessage("Unable to deserialize event.");
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
        await act.Should().ThrowAsync<EntraDeserializationException>().WithMessage("Failed to deserialize event.");
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
        var json = """
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

    [Fact]
    public async Task WriteOkAsync_Success()
    {
        // Arrange
        var testProperty = _fixture.Create<string>();

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var response = new TestResponse { TestProperty = testProperty };

        // Act
        await _sut.WriteOkAsync(context, response);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        context.Response.ContentType.Should().Be("application/json");

        var deserialized = await TestUtils.ReadJson<TestResponse>(context.Response.Body);
        deserialized.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task WriteErrorAsync_Success()
    {
        // Arrange
        var statusCode = StatusCodes.Status400BadRequest;

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var response = new EntraErrorResponse { Error = _fixture.Create<string>(), Details = _fixture.Create<string>() };

        // Act
        await _sut.WriteErrorAsync(context, statusCode, response);

        // Assert
        context.Response.StatusCode.Should().Be(statusCode);
        context.Response.ContentType.Should().Be("application/json");

        var deserialized = await TestUtils.ReadJson<EntraErrorResponse>(context.Response.Body);
        deserialized.Should().BeEquivalentTo(response);
    }
}
