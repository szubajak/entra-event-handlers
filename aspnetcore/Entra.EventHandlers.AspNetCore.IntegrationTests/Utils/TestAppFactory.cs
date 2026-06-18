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

            services.AddSingleton<TestTokenIssuanceStartHandler>();
            services.AddSingleton<ITokenIssuanceStartHandler>(sp => sp.GetRequiredService<TestTokenIssuanceStartHandler>());
            services.AddTransient<TokenIssuanceStartEndpoint>();

            services.AddSingleton<TestEmailOtpSendHandler>();
            services.AddSingleton<IEmailOtpSendHandler>(sp => sp.GetRequiredService<TestEmailOtpSendHandler>());
            services.AddTransient<EmailOtpSendEndpoint>();

            services.AddSingleton<TestPasswordSubmitHandler>();
            services.AddSingleton<IPasswordSubmitHandler>(sp => sp.GetRequiredService<TestPasswordSubmitHandler>());
            services.AddTransient<PasswordSubmitEndpoint>();

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
                endpoints.ServiceProvider.GetRequiredService<TokenIssuanceStartEndpoint>().Map(endpoints);
                endpoints.ServiceProvider.GetRequiredService<EmailOtpSendEndpoint>().Map(endpoints);
                endpoints.ServiceProvider.GetRequiredService<PasswordSubmitEndpoint>().Map(endpoints);
            });
        });
    }
}
