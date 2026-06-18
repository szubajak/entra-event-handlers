namespace Entra.EventHandlers.AspNetCore.TestHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRouting();
        builder.Services.AddLogging();

        var app = builder.Build();

        app.MapGet("/", () => "OK");

        app.Run();
    }
}
