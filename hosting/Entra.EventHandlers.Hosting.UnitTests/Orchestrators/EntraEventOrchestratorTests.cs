using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using NSubstitute;

namespace Entra.EventHandlers.Hosting.UnitTests.Orchestrators;

public class EntraEventOrchestratorTests
{
    private readonly EntraEventOrchestrator _sut;

    private readonly IEntraEventHandlerResolver _resolver;

    public EntraEventOrchestratorTests()
    {
        _resolver = Substitute.For<IEntraEventHandlerResolver>();

        _sut = new EntraEventOrchestrator(_resolver);
    }

    [Fact]
    public async Task DispatchAsync_AttributeCollectionStartEvent_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var evt = fixture.Create<AttributeCollectionStartEvent>();
        var response = new AttributeCollectionStartResponse();
        var cts = new CancellationTokenSource();

        var handler = Substitute.For<IEntraEventHandler<AttributeCollectionStartEvent, AttributeCollectionStartResponse>>();
        handler.HandleAsync(evt, cts.Token).Returns(response);

        _resolver.Resolve<AttributeCollectionStartEvent, AttributeCollectionStartResponse>()
            .Returns(handler);

        // Act
        var result = await _sut.DispatchAsync(evt, cts.Token);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task DispatchAsync_AttributeCollectionSubmitEvent_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var evt = fixture.Create<AttributeCollectionSubmitEvent>();
        var response = new AttributeCollectionSubmitResponse();
        var cts = new CancellationTokenSource();

        var handler = Substitute.For<IEntraEventHandler<AttributeCollectionSubmitEvent, AttributeCollectionSubmitResponse>>();
        handler.HandleAsync(evt, cts.Token).Returns(response);

        _resolver.Resolve<AttributeCollectionSubmitEvent, AttributeCollectionSubmitResponse>()
            .Returns(handler);

        // Act
        var result = await _sut.DispatchAsync(evt, cts.Token);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task DispatchAsync_TokenIssuanceStartEvent_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var evt = fixture.Create<TokenIssuanceStartEvent>();
        var response = new TokenIssuanceStartResponse();
        var cts = new CancellationTokenSource();

        var handler = Substitute.For<IEntraEventHandler<TokenIssuanceStartEvent, TokenIssuanceStartResponse>>();
        handler.HandleAsync(evt, cts.Token).Returns(response);

        _resolver.Resolve<TokenIssuanceStartEvent, TokenIssuanceStartResponse>()
            .Returns(handler);

        // Act
        var result = await _sut.DispatchAsync(evt, cts.Token);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task DispatchAsync_EmailOtpSendEvent_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var evt = fixture.Create<EmailOtpSendEvent>();
        var response = new EmailOtpSendResponse();
        var cts = new CancellationTokenSource();

        var handler = Substitute.For<IEntraEventHandler<EmailOtpSendEvent, EmailOtpSendResponse>>();
        handler.HandleAsync(evt, cts.Token).Returns(response);

        _resolver.Resolve<EmailOtpSendEvent, EmailOtpSendResponse>()
            .Returns(handler);

        // Act
        var result = await _sut.DispatchAsync(evt, cts.Token);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task DispatchAsync_PasswordSubmitEvent_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var evt = fixture.Create<PasswordSubmitEvent>();
        var response = new PasswordSubmitResponse();
        var cts = new CancellationTokenSource();

        var handler = Substitute.For<IEntraEventHandler<PasswordSubmitEvent, PasswordSubmitResponse>>();
        handler.HandleAsync(evt, cts.Token).Returns(response);

        _resolver.Resolve<PasswordSubmitEvent, PasswordSubmitResponse>()
            .Returns(handler);

        // Act
        var result = await _sut.DispatchAsync(evt, cts.Token);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task DispatchAsync_UnknownEvent_ThrowsNotSupportedException()
    {
        // Arrange
        var evt = new TestEvent();

        // Act
        var act = () => _sut.DispatchAsync(evt, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotSupportedException>()
            .WithMessage("Unsupported event type*");
    }
}