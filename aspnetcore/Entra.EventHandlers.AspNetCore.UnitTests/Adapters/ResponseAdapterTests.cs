using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AspNetCore.Adapters;
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
    public async Task WriteBadRequestAsync_Success()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var response = new EntraErrorResponse
        {
            Error = _fixture.Create<string>(),
            Details = _fixture.Create<string>()
        };

        // Act
        await _sut.WriteBadRequestAsync(context, response);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        context.Response.ContentType.Should().Be("application/json");

        var deserialized = await TestUtils.ReadJson<EntraErrorResponse>(context.Response.Body);
        deserialized.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task WriteServerErrorAsync_Success()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var response = new EntraErrorResponse
        {
            Error = _fixture.Create<string>(),
            Details = _fixture.Create<string>()
        };

        // Act
        await _sut.WriteServerErrorAsync(context, response);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        context.Response.ContentType.Should().Be("application/json");

        var deserialized = await TestUtils.ReadJson<EntraErrorResponse>(context.Response.Body);
        deserialized.Should().BeEquivalentTo(response);
    }
}
