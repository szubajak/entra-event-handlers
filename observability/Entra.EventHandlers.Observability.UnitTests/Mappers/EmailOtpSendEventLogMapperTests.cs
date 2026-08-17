using AutoFixture;
using AutoFixture.Kernel;
using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Observability.Mappers;
using FluentAssertions;

namespace Entra.EventHandlers.Observability.UnitTests.Mappers;

public class EmailOtpSendEventLogMapperTests
{
    private readonly EmailOtpSendEventLogMapper _sut;

    public EmailOtpSendEventLogMapperTests()
    {
        _sut = new EmailOtpSendEventLogMapper();
    }

    [Fact]
    public async Task Map_Success()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customizations.Add(new TypeRelay(typeof(EntraAction), typeof(ContinueAction)));

        var request = fixture.Create<EmailOtpSendEvent>();
        var response = fixture.Create<EmailOtpSendResponse>();

        // Act
        var result = _sut.Map(request, response);

        // Assert
        result.Should().NotBeNull();
        result.TenantId.Should().Be(request.Data.TenantId);
    }
}
