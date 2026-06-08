using Entra.EventHandlers.AspNetCore.DI;
using Entra.EventHandlers.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Entra Event Handlers
builder.Services.AddEntraEventHandlers();

var app = builder.Build();

// Recommended: Map Entra Router (one endpoint to handle all events)
app.MapEntraRouter();

// Optional: Map Specific Entra Events 
app.MapEntraAttributeCollectionStart();
app.MapEntraAttributeCollectionSubmit();
app.MapEntraTokenIssuanceStart();
app.MapEntraEmailOtpSend();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();