using Entra.EventHandlers.AzureFunctions.DI;
using Entra.EventHandlers.Interfaces;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Common.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// PasswordSubmitHandler require service to decrypt encrypted password context
builder.Services.AddTransient<IPasswordContextCryptoService, PasswordContextCryptoService>();

// Add Entra Event Handlers
builder.Services.AddEntraEventHandlers();

builder.Build().Run();
