using AutoFixture;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.UnitTests.Utils;
using Entra.EventHandlers.Hosting.Errors;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using NSubstitute;
using System.Net;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Adapters;

public class ResponseAdapterTests
{
    private readonly ResponseAdapter _sut;

    private readonly Fixture _fixture = new();

    public ResponseAdapterTests()
    {
        _sut = new ResponseAdapter();
    }

    [Fact]
    public async Task FromAsync_Success()
    {
        // Arrange
        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, new MemoryStream());

        var response = new TestResponse { TestProperty = _fixture.Create<string>() };

        // Act
        var result = await _sut.FromAsync(req, response);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Headers.GetValues("Content-Type")
          .Should()
          .Contain("application/json");

        var deserialized = await TestUtils.ReadJson<TestResponse>(result.Body);
        deserialized.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task FromErrorAsync_Success()
    {
        // Arrange
        var context = Substitute.For<FunctionContext>();
        var req = new TestHttpRequestData(context, new MemoryStream());

        var statusCode = HttpStatusCode.RequestTimeout;

        var response = new EntraErrorResponse
        { 
            Error = _fixture.Create<string>(),
            Details = _fixture.Create<string>()
        };

        // Act
        var result = await _sut.FromErrorAsync(req, statusCode, response);

        // Assert
        result.StatusCode.Should().Be(statusCode);
        result.Headers.GetValues("Content-Type")
          .Should()
          .Contain("application/json");

        var deserialized = await TestUtils.ReadJson<EntraErrorResponse>(result.Body);
        deserialized.Should().BeEquivalentTo(response);
    }
}