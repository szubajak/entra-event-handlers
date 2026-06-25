using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Abstractions;

public class EntraFunctionBaseTests
{
    private readonly TestEntraFunctionBase _sut;

    private readonly TestLogger _logger;
    private readonly IRequestAdapter _requestAdapter;
    private readonly IResponseAdapter _responseAdapter;

    public EntraFunctionBaseTests()
    {
        _logger = new TestLogger();
        _requestAdapter = Substitute.For<IRequestAdapter>();
        _responseAdapter = Substitute.For<IResponseAdapter>();

        _sut = new TestEntraFunctionBase(_logger, _requestAdapter, _responseAdapter);
    }

    [Fact]
    public async Task InvokeAsync_Calls_ExecuteAsync()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var req = Substitute.For<HttpRequestData>(ctx);

        bool executed = false;
        _sut.ExecuteDelegate = _ =>
        {
            executed = true;
            return Task.FromResult(Substitute.For<HttpResponseData>(ctx));
        };

        // Act
        await _sut.Invoke(req);

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_PassesCancellationTokenThroughPipeline()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var cts = new CancellationTokenSource();
        ctx.CancellationToken.Returns(cts.Token);

        var req = Substitute.For<HttpRequestData>(ctx);

        CancellationToken? captured = null;

        _sut.ExecuteDelegate = r =>
        {
            captured = r.FunctionContext.CancellationToken;
            return Task.FromResult(Substitute.For<HttpResponseData>(ctx));
        };

        // Act
        await _sut.Invoke(req);

        // Assert
        captured.Should().Be(cts.Token);
    }

    [Fact]
    public async Task InvokeAsync_EntraException_MapsToBadRequest()
    {
        // Arrange
        var fixture = new Fixture();

        var ctx = Substitute.For<FunctionContext>();
        var req = Substitute.For<HttpRequestData>(ctx);

        var exception = new EntraValidationException(fixture.Create<string>());

        _sut.ExecuteDelegate = _ => throw exception;

        // Act
        await _sut.Invoke(req);

        // Assert
        await _responseAdapter.Received(1)
            .BadRequestAsync(req, Arg.Any<EntraErrorResponse>());

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Handled expected Entra exception."));
    }

    [Fact]
    public async Task InvokeAsync_UnexpectedException_MapsToServerError()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var req = Substitute.For<HttpRequestData>(ctx);

        var exception = new InvalidOperationException();

        _sut.ExecuteDelegate = _ => throw exception;

        // Act
        await _sut.InvokeAsync(req);

        // Assert
        await _responseAdapter.Received(1)
            .ServerErrorAsync(req, Arg.Any<EntraErrorResponse>());

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Error &&
            e.Exception == exception &&
            e.Message.Contains("Unhandled exception while processing Entra event."));
    }
}
