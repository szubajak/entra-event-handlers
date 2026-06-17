using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Endpoints;
using Entra.EventHandlers.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<TestAttributeCollectionStartHandler>();
            services.AddSingleton<IAttributeCollectionStartHandler>(sp => sp.GetRequiredService<TestAttributeCollectionStartHandler>());
            services.AddTransient<AttributeCollectionStartEndpoint>();

            services.AddSingleton<TestAttributeCollectionSubmitHandler>();
            services.AddSingleton<IAttributeCollectionSubmitHandler>(sp => sp.GetRequiredService<TestAttributeCollectionSubmitHandler>());
            services.AddTransient<AttributeCollectionSubmitEndpoint>();

            services.AddSingleton<TestEmailOtpSendHandler>();
            services.AddSingleton<IEmailOtpSendHandler>(sp => sp.GetRequiredService<TestEmailOtpSendHandler>());
            services.AddTransient<EmailOtpSendEndpoint>();

            services.AddSingleton<IRequestAdapter, TestRequestAdapter>();
            services.AddSingleton<IResponseAdapter, TestResponseAdapter>();
        });

        builder.Configure(app =>
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.ServiceProvider.GetRequiredService<AttributeCollectionStartEndpoint>().Map(endpoints);
                endpoints.ServiceProvider.GetRequiredService<AttributeCollectionSubmitEndpoint>().Map(endpoints);
                endpoints.ServiceProvider.GetRequiredService<EmailOtpSendEndpoint>().Map(endpoints);
            });
        });
    }
}
