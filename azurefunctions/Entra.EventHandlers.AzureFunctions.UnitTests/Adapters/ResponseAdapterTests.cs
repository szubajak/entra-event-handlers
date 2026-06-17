using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.UnitTests.Utils;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NSubstitute;
using System.Net;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Adapters;

public class ResponseAdapterTests
{
    private readonly ResponseAdapter _sut;

    public ResponseAdapterTests()
    {
        _sut = new ResponseAdapter();
    }

    [Fact]
    public async Task From_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, new MemoryStream());

        var response = new TestResponse { TestProperty = fixture.Create<string>() };

        // Act
        var result = await _sut.From(req, response);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Headers.GetValues("Content-Type")
          .Should()
          .Contain("application/json");

        var deserialized = await TestUtils.ReadJson<TestResponse>(result.Body);
        deserialized.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task BadRequest_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, new MemoryStream());

        var response = new EntraErrorResponse
        { 
            Error = fixture.Create<string>(),
            Details = fixture.Create<string>()
        };

        // Act
        var result = await _sut.BadRequest(req, response);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Headers.GetValues("Content-Type")
          .Should()
          .Contain("application/json");

        var deserialized = await TestUtils.ReadJson<EntraErrorResponse>(result.Body);
        deserialized.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task ServerError_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, new MemoryStream());

        var response = new EntraErrorResponse
        {
            Error = fixture.Create<string>(),
            Details = fixture.Create<string>()
        };

        // Act
        var result = await _sut.ServerError(req, response);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Headers.GetValues("Content-Type")
          .Should()
          .Contain("application/json");

        var deserialized = await TestUtils.ReadJson<EntraErrorResponse>(result.Body);
        deserialized.Should().BeEquivalentTo(response);
    }
}

public static class HttpHeadersCollectionExtensions
{
    public static IEnumerable<string> GetValues(this HttpHeadersCollection headers, string name)
    {
        headers.TryGetValues(name, out var values);
        return values ?? [];
    }
}