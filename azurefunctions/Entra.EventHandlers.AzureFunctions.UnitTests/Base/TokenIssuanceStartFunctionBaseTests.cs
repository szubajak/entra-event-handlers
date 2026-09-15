using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.UnitTests.Utils;
using Entra.EventHandlers.TestData;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Base;

public class TokenIssuanceStartFunctionBaseTests
{
    private readonly TestTokenIssuanceStartFunctionBase _sut;

    private readonly Fixture _fixture = new();

    private readonly ILogger _logger;
    private readonly ITokenIssuanceStartHandler _handler;
    private readonly IRequestAdapter _requestAdapter;
    private readonly IResponseAdapter _responseAdapter;

    public TokenIssuanceStartFunctionBaseTests()
    {
        _logger = Substitute.For<ILogger>();
        _handler = Substitute.For<ITokenIssuanceStartHandler>();
        _requestAdapter = Substitute.For<IRequestAdapter>();
        _responseAdapter = Substitute.For<IResponseAdapter>();

        _sut = new TestTokenIssuanceStartFunctionBase(_logger, _handler, _requestAdapter, _responseAdapter);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Call_Handler_And_Return_Response()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var evt = TestEvents.CreateTokenIssuanceStartEvent(_fixture);

        _requestAdapter
            .ReadEventAsync<TokenIssuanceStartEvent>(request)
            .Returns(evt);

        var entraResponse = TestResponses.CreateTokenIssuanceStartResponse();
        var handlerResult = new EntraHandlerResult<TokenIssuanceStartResponse>(entraResponse);

        _handler.HandleAsync(evt, request.FunctionContext.CancellationToken)
            .Returns(handlerResult);

        _responseAdapter.FromAsync(request, handlerResult.Response)
            .Returns(response);

        // Act
        var result = await _sut.InvokeAsync(request);

        // Assert
        result.Should().Be(response);
        await _handler.Received(1).HandleAsync(evt, request.FunctionContext.CancellationToken);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Invoke_OnException_When_Handler_Returns_Exception()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var evt = TestEvents.CreateTokenIssuanceStartEvent(_fixture);

        var exception = new InvalidOperationException("Invalid!");

        _requestAdapter
            .ReadEventAsync<TokenIssuanceStartEvent>(request)
            .Returns(evt);

        var entraResponse = TestResponses.CreateTokenIssuanceStartResponse();
        var handlerResult = new EntraHandlerResult<TokenIssuanceStartResponse>(entraResponse, exception);

        _handler.HandleAsync(evt, request.FunctionContext.CancellationToken)
            .Returns(handlerResult);

        _responseAdapter.FromAsync(request, handlerResult.Response)
            .Returns(response);

        // Act
        var result = await _sut.InvokeAsync(request);

        // Assert
        _sut.ExceptionCalled.Should().BeTrue();
        result.Should().Be(response);
    }
}
