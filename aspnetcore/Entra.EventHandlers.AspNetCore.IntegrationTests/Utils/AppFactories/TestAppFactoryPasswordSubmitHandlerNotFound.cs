using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Endpoints;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.Resolvers;
using Entra.EventHandlers.Hosting.Resolvers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.AppFactories;

public class TestAppFactoryPasswordSubmitHandlerNotFound : TestAppFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IPasswordSubmitHandler>();
            services.RemoveAll<TestPasswordSubmitHandler>();
            services.RemoveAll<PasswordSubmitEndpoint>();

            services.AddSingleton<IEntraEventHandlerResolver, TestResolverPasswordSubmitHandlerNotFound>();
        });

        builder.Configure(app =>
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.ServiceProvider.GetRequiredService<AttributeCollectionStartEndpoint>().Map(endpoints);
                endpoints.ServiceProvider.GetRequiredService<AttributeCollectionSubmitEndpoint>().Map(endpoints);
                endpoints.ServiceProvider.GetRequiredService<TokenIssuanceStartEndpoint>().Map(endpoints);
                endpoints.ServiceProvider.GetRequiredService<EmailOtpSendEndpoint>().Map(endpoints);

                endpoints.ServiceProvider.GetRequiredService<EntraEventRouterEndpoint>().Map(endpoints);
            });
        });
    }
}
