using AutoFixture;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Errors;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Adapters;

public class ResponseAdapterTests
{
    private readonly ResponseAdapter _sut;

    private readonly Fixture _fixture = new();

    public ResponseAdapterTests()
    {
        _sut = new ResponseAdapter();
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

        var response = new EntraErrorResponse
        {
            Error = _fixture.Create<string>(),
            Details = _fixture.Create<string>()
        };

        // Act
        await _sut.WriteErrorAsync(context, statusCode, response);

        // Assert
        context.Response.StatusCode.Should().Be(statusCode);
        context.Response.ContentType.Should().Be("application/json");

        var deserialized = await TestUtils.ReadJson<EntraErrorResponse>(context.Response.Body);
        deserialized.Should().BeEquivalentTo(response);
    }
}
