using Entra.EventHandlers.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;

public class TestAppFactoryPasswordSubmitHandlerThrowsUnexpected : TestAppFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<TestPasswordSubmitHandler>();
            services.RemoveAll<IPasswordSubmitHandler>();

            services.AddSingleton<IPasswordSubmitHandler, TestPasswordSubmitHandlerThrowsUnexpected>();
        });
    }
}
