using Entra.EventHandlers.AzureFunctions.DI;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddEntraEventHandlers();

builder.Build().Run();
