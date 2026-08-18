using Entra.EventHandlers.Observability.Factories;
using Entra.EventHandlers.Observability.Interfaces;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Entra.EventHandlers.Observability.UnitTests.Factories;

public class EventLogMapperFactoryTests
{
    [Fact]
    public void Get_Should_Return_Registered_Mapper()
    {
        // Arrange
        var services = new ServiceCollection();

        var mapper = Substitute.For<IEventLogMapper<TestEvent, TestResponse>>();
        services.AddSingleton(mapper);

        var provider = services.BuildServiceProvider();
        var sut = new EventLogMapperFactory(provider);

        // Act
        var result = sut.Get<TestEvent, TestResponse>();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Get_Should_Throw_If_Missing_Mapper()
    {
        // Arrange
        var services = new ServiceCollection();

        var provider = services.BuildServiceProvider();
        var sut = new EventLogMapperFactory(provider);

        // Act
        var act = () => sut.Get<TestEvent, TestResponse>();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"No mapper registered for {typeof(TestEvent).Name} → {typeof(TestResponse).Name}");
    }
}
