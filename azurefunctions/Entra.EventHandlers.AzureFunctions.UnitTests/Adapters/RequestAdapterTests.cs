using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.UnitTests.Utils;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using NSubstitute;
using System.Text;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Adapters;

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
        var ctx = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(ctx, new MemoryStream(Encoding.UTF8.GetBytes("")));

        // Act
        var act = async () => await _sut.ReadEvent<TestEvent>(req);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Request body is empty.");
    }

    [Fact]
    public async Task ReadEvent_WhenJsonIsInvalid_ThrowsInvalidJsonException()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(ctx, new MemoryStream(Encoding.UTF8.GetBytes("{ invalid json ")));

        // Act
        var act = async () => await _sut.ReadEvent<TestEvent>(req);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Invalid JSON payload.");
    }

    [Fact]
    public async Task ReadEvent_WhenJsonDeserializesToNull_ThrowsUnableToDeserialize()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(ctx, new MemoryStream(Encoding.UTF8.GetBytes("null")));

        // Act
        var act = async () => await _sut.ReadEvent<TestEvent>(req);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Unable to deserialize event.");
    }

    [Fact]
    public async Task ReadEvent_WhenUnexpectedExceptionOccurs_ThrowsFailedToDeserialize()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(ctx, new ThrowingStream());

        // Act
        var act = async () => await _sut.ReadEvent<TestEvent>(req);

        // Assert
        await act.Should()
            .ThrowAsync<EntraDeserializationException>()
            .WithMessage("Failed to deserialize event.");
    }

    [Fact]
    public async Task ReadEvent_WhenJsonIsValid_ReturnsDeserializedEvent()
    {
        // Arrange
        var expectedResult = new TestEvent();

        var body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, body);

        // Act
        var result = await _sut.ReadEvent<TestEvent>(req);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }
}
