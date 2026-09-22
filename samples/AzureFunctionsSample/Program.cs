using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.DI;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Common.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// PasswordSubmitHandler require service to decrypt encrypted password context
builder.Services.AddTransient<IPasswordContextDecryptor, PasswordContextDecryptor>();

// Add Entra Event Handlers
builder.Services.AddEntraEventHandlers();

await builder.Build().RunAsync();
