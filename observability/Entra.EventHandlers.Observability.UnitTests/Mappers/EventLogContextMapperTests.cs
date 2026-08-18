using AutoFixture;
using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Mappers;
using Entra.EventHandlers.Observability.Models;
using FluentAssertions;

namespace Entra.EventHandlers.Observability.UnitTests.Mappers;

public class EventLogContextMapperTests
{
    private readonly EventLogContextMapper _sut;

    public EventLogContextMapperTests()
    {
        _sut = new EventLogContextMapper();
    }

    [Fact]
    public async Task Map_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var validCustomLogsCount = 5;

        var customLogsEntries = fixture.CreateMany<CustomLogEntry>(validCustomLogsCount).ToList();

        var expectedCustomLogs = customLogsEntries.Select(x => new
        {
            A = x.Timestamp,
            B = x.Data
        }).ToList();

        // Add two unsupported / broken logs
        customLogsEntries.Add(null!);
        customLogsEntries.Add(new CustomLogEntry() { Data = null! });

        var ctx = fixture.Build<EventLogContext>()
            .Do(x => customLogsEntries.ForEach(x.CustomLogEntries.Add))
            .Create();

        // Act
        var result = _sut.Map(ctx);

        // Assert
        result.Should().NotBeNull();

        var defaultLog = result.DefaultLog;
        defaultLog.Should().NotBeNull();
        defaultLog.TenantId.Should().Be(ctx.DefaultLog.TenantId);

        var customLogs = result.CustomLogs;
        customLogs.Should().NotBeNullOrEmpty();

        var resultedCustomLogs = customLogs.Select(x => new
        {
            A = x.Timestamp,
            B = x.Data
        });

        resultedCustomLogs.Should().BeEquivalentTo(expectedCustomLogs);

        customLogs.Count.Should().Be(validCustomLogsCount);
    }
}
