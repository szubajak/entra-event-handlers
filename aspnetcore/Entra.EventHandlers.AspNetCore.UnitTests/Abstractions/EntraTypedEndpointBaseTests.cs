using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Interfaces;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Abstractions;

public class EntraTypedEndpointBaseTests
{
    private readonly TestTypedEntraEndpointBase _sut;

    private readonly ILogger _logger;
    private readonly IRequestAdapter _requestAdapter;
    private readonly IResponseAdapter _responseAdapter;
    private readonly IEntraEventHandlerResolver _resolver;

    public EntraTypedEndpointBaseTests()
    {
        _logger = Substitute.For<ILogger>();
        _requestAdapter = Substitute.For<IRequestAdapter>();
        _responseAdapter = Substitute.For<IResponseAdapter>();
        _resolver = Substitute.For<IEntraEventHandlerResolver>();

        _sut = new TestTypedEntraEndpointBase(_logger, _requestAdapter, _responseAdapter, _resolver);
    }

    [Fact]
    public async Task InvokeAsync_UsesResolver_AndWritesOk()
    {
        // Arrange
        var ctx = new DefaultHttpContext();

        var evt = new TestEvent();
        var response = new TestResponse();

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();

        _requestAdapter
            .ReadEventAsync<TestEvent>(ctx)
            .Returns(evt);

        _resolver
            .Resolve<TestEvent, TestResponse>()
            .Returns(handler);

        handler
            .HandleAsync(evt, ctx.RequestAborted)
            .Returns(new EntraHandlerResult<TestResponse>(response));

        _responseAdapter
            .WriteOkAsync(ctx, response)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _resolver
            .Received(1)
            .Resolve<TestEvent, TestResponse>();

        _ = handler.Received(1).HandleAsync(evt, ctx.RequestAborted);
        _ = _responseAdapter.Received(1).WriteOkAsync(ctx, response);
    }

    [Fact]
    public async Task InvokeAsync_WhenResultHasException_CallsExceptionHandler()
    {
        // Arrange
        var services = new ServiceCollection();
        var exceptionHandler = Substitute.For<IEntraExceptionHandler>();
        services.AddSingleton(exceptionHandler);
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext
        {
            RequestServices = provider
        };

        var evt = new TestEvent();
        var response = new TestResponse();
        var exception = new InvalidOperationException("boom");

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();

        _requestAdapter
            .ReadEventAsync<TestEvent>(ctx)
            .Returns(evt);

        _resolver
            .Resolve<TestEvent, TestResponse>()
            .Returns(handler);

        handler
            .HandleAsync(evt, ctx.RequestAborted)
            .Returns(new EntraHandlerResult<TestResponse>(response, exception));

        _responseAdapter
            .WriteOkAsync(ctx, response)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _resolver
            .Received(1)
            .Resolve<TestEvent, TestResponse>();

        _ = exceptionHandler.Received(1).HandleAsync(exception);
        _ = _responseAdapter.Received(1).WriteOkAsync(ctx, response);
    }
}
