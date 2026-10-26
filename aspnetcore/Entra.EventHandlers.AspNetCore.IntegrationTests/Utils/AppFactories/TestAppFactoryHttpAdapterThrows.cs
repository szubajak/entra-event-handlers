using Entra.EventHandlers.AspNetCore.Adapters;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;

public class TestAppFactoryHttpAdapterThrows : TestAppFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IHttpAdapter>();
            services.AddSingleton<IHttpAdapter, TestHttpAdapterThrows>();
        });
    }
}
