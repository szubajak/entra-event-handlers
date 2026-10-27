using System.Net;
using System.Text;
using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.UnitTests.Utils;
using Entra.EventHandlers.Hosting.Errors;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using NSubstitute;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Adapters;

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
        var ctx = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(ctx, new MemoryStream(Encoding.UTF8.GetBytes("")));

        // Act
        var act = async () => await _sut.ReadEventAsync<TestEvent>(req);

        // Assert
        await act.Should().ThrowAsync<EntraDeserializationException>().WithMessage("Request body is empty.");
    }

    [Fact]
    public async Task ReadEventAsync_WhenJsonIsInvalid_ThrowsInvalidJsonException()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(ctx, new MemoryStream(Encoding.UTF8.GetBytes("{ invalid json ")));

        // Act
        var act = async () => await _sut.ReadEventAsync<TestEvent>(req);

        // Assert
        await act.Should().ThrowAsync<EntraDeserializationException>().WithMessage("Invalid JSON payload.");
    }

    [Fact]
    public async Task ReadEventAsync_WhenJsonDeserializesToNull_ThrowsUnableToDeserialize()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(ctx, new MemoryStream(Encoding.UTF8.GetBytes("null")));

        // Act
        var act = async () => await _sut.ReadEventAsync<TestEvent>(req);

        // Assert
        await act.Should().ThrowAsync<EntraDeserializationException>().WithMessage("Unable to deserialize event.");
    }

    [Fact]
    public async Task ReadEventAsync_WhenUnexpectedExceptionOccurs_ThrowsFailedToDeserialize()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(ctx, new ThrowingStream());

        // Act
        var act = async () => await _sut.ReadEventAsync<TestEvent>(req);

        // Assert
        await act.Should().ThrowAsync<EntraDeserializationException>().WithMessage("Failed to deserialize event.");
    }

    [Fact]
    public async Task ReadEventAsync_WhenJsonIsValid_ReturnsDeserializedEvent()
    {
        // Arrange
        var expectedResult = new TestEvent();

        var body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, body);

        // Act
        var result = await _sut.ReadEventAsync<TestEvent>(req);

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

        var body = new MemoryStream(Encoding.UTF8.GetBytes(json));

        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, body);

        // Act
        var result = await _sut.ReadEventAsync(req);

        // Assert
        result.Should().BeAssignableTo<EntraEvent>();
    }

    [Fact]
    public async Task CreateResponseAsync_Success()
    {
        // Arrange
        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, new MemoryStream());

        var response = new TestResponse { TestProperty = _fixture.Create<string>() };

        // Act
        var result = await _sut.CreateResponseAsync(req, response);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Headers.GetValues("Content-Type").Should().Contain("application/json");

        var deserialized = await TestUtils.ReadJson<TestResponse>(result.Body);
        deserialized.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task CreateErrorResponseAsync_Success()
    {
        // Arrange
        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, new MemoryStream());

        var statusCode = HttpStatusCode.RequestTimeout;

        var response = new EntraErrorResponse { Error = _fixture.Create<string>(), Details = _fixture.Create<string>() };

        // Act
        var result = await _sut.CreateErrorResponseAsync(req, statusCode, response);

        // Assert
        result.StatusCode.Should().Be(statusCode);
        result.Headers.GetValues("Content-Type").Should().Contain("application/json");

        var deserialized = await TestUtils.ReadJson<EntraErrorResponse>(result.Body);
        deserialized.Should().BeEquivalentTo(response);
    }
}
