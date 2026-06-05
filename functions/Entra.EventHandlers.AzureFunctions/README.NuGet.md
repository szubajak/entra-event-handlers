# Entra.EventHandlers.AzureFunctions

**Azure Functions hosting adapter for Microsoft Entra External ID Authentication Event Handlers.**  
Provides minimal‑boilerplate hosting, full DI support, structured error handling, and complete testability.

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

---

## ✨ Features

- 🚀 **Single Function → Multiple Entra Event Types**  
- 🔄 **Automatic request deserialization & response serialization**  
- 🧩 **Dynamic handler resolution via DI**  
- 🛡 **Structured error mapping (400/500)**  
- 🧪 **Fully unit‑testable (no static calls, no real HTTP streams)**  
- 🪶 **Minimal boilerplate**

---

## 🧠 Core Router

```csharp
public abstract class EntraEventRouterFunctionBase(
    ILogger<EntraEventRouterFunctionBase> logger,
    IEntraEventHandlerResolver resolver,
    IHttpRequestAdapter requestAdapter,
    IHttpResponseAdapter responseAdapter)
{
    private readonly ILogger<EntraEventRouterFunctionBase> _logger = logger;
    private readonly IEntraEventHandlerResolver _resolver = resolver;
    private readonly IHttpRequestAdapter _requestAdapter = requestAdapter;
    private readonly IHttpResponseAdapter _responseAdapter = responseAdapter;

    protected async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        try
        {
            var evt = await _requestAdapter.ReadEvent(req);
            var handler = _resolver.Resolve(evt.GetType());

            var response = await ((dynamic)handler).Handle((dynamic)evt, context.CancellationToken);
            return await _responseAdapter.From(req, response);
        }
        catch (Exception ex) when (ex is EntraValidationException or EntraDeserializationException or EntraHandlerNotFoundException)
        {
            _logger.LogWarning(ex, "Handled expected Entra exception.");

            var code = ex switch
            {
                EntraValidationException => EntraErrorCodes.ValidationError,
                EntraDeserializationException => EntraErrorCodes.DeserializationError,
                EntraHandlerNotFoundException => EntraErrorCodes.HandlerNotFound,
                _ => throw new InvalidOperationException("Unreachable: catch filter guarantees only known Entra exceptions.")
            };

            return await _responseAdapter.BadRequest(
                req,
                new EntraErrorResponse
                {
                    Error = code,
                    Details = ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing Entra event.");

            return await _responseAdapter.ServerError(
                req,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.UnhandledException,
                    Details = "An unexpected error occurred."
                });
        }
    }
}
```

---

## 🧩 Minimal Azure Function

```csharp
public sealed class EntraRouterFunction : EntraEventRouterFunctionBase
{
    public EntraRouterFunction(
        ILogger<EntraEventRouterFunctionBase> logger,
        IEntraEventHandlerResolver resolver,
        IHttpRequestAdapter requestAdapter,
        IHttpResponseAdapter responseAdapter)
        : base(logger, resolver, requestAdapter, responseAdapter) {}

    [Function("EntraRouter")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext ctx)
        => Run(req, ctx);
}
```

---

## 🛠 Dependency Injection

```csharp
public static class EntraEventHandlersFunctionExtensions
{
    public static IServiceCollection AddEntraEventHandlersForFunctions(this IServiceCollection services)
    {
        services.AddSingleton<IHttpRequestAdapter, HttpRequestAdapter>();
        services.AddSingleton<IHttpResponseAdapter, HttpResponseAdapter>();
        services.AddSingleton<IEntraEventHandlerResolver, EntraEventHandlerResolver>();

        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo(typeof(IEntraEventHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
```

---

## 🧪 Unit Testing Example

```csharp
[Fact]
public async Task Run_WhenDeserializationFails_ReturnsBadRequest()
{
    // Arrange
    var ctx = Substitute.For<FunctionContext>();
    var request = Substitute.For<HttpRequestData>(ctx);
    var response = Substitute.For<HttpResponseData>(ctx);

    var exception = new EntraDeserializationException("bad");
    _requestAdapter.ReadEvent(request).Throws(exception);

    _responseAdapter
        .BadRequest(request, Arg.Any<EntraErrorResponse>())
        .Returns(response);

    // Act
    var result = await _sut.RunAsync(request, ctx);

    // Assert
    _responseAdapter.Received(1).BadRequest(
        request,
        Arg.Is<EntraErrorResponse>(e =>
            e.Error == EntraErrorCodes.DeserializationError &&
            e.Details == "bad"));

    _logger.Entries.Should().ContainSingle(e =>
        e.Level == LogLevel.Warning &&
        e.Exception == exception);
}
```

---

## 📦 Optional: Single‑Event Base Classes

```csharp
public abstract class TokenIssuanceStartFunctionBase(
    ITokenIssuanceStartHandler handler,
    IHttpRequestAdapter requestAdapter,
    IHttpResponseAdapter responseAdapter)
{
    private readonly ITokenIssuanceStartHandler _handler = handler;
    private readonly IHttpRequestAdapter _requestAdapter = requestAdapter;
    private readonly IHttpResponseAdapter _responseAdapter = responseAdapter;

    protected async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await _requestAdapter.ReadEvent<TokenIssuanceStartEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await _responseAdapter.From(req, response);
    }
}
```

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

A commercial license is required for production use by organizations with more than 5 employees.

### Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.

---

## 📚 Documentation

Full documentation, examples, and production templates will be available in the main repository as the ecosystem evolves.
