using Entra.EventHandlers.AspNetCore.DI;
using Entra.EventHandlers.AspNetCore.Extensions;
using Entra.EventHandlers.Interfaces;
using Sample.Common.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// PasswordSubmitHandler require service to decrypt encrypted password context
builder.Services.AddTransient<IPasswordContextCryptoService, PasswordContextCryptoService>();

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
app.MapPasswordSubmit();
app.MapVerifiedIdClaimValidation();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.RunAsync();