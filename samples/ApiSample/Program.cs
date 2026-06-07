using Entra.EventHandlers.AspNetCore.DI;
using Entra.EventHandlers.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Entra Event Handlers
builder.Services.AddEntraEventHandlers();

var app = builder.Build();

// Map Entra Router
app.MapEntraRouter();

// Map Specific Entra Event
app.MapEntraTokenIssuanceStart();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();