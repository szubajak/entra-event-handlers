# Entra.EventHandlers.AspNetCore

**ASP.NET Core hosting adapter for Microsoft Entra External ID Authentication Event Handlers.**  
Provides minimal‑boilerplate hosting, full DI support, unified exception handling, structured logging, and complete testability.

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

---

## ✨ Features

- 🚀 Single endpoint can host **multiple Entra event types**  
- 🔄 Automatic request deserialization & response serialization  
- 🧩 Dynamic handler resolution via DI  
- 🛡 Structured error mapping (400/500)  
- 🧪 Fully unit‑testable  
- 🪶 Minimal boilerplate  
- 🧭 Clean endpoint mapping extensions

---

## 🚀 Quick Start

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntraEventHandlers();

var app = builder.Build();

// Multi‑event router (recommended)
app.MapEntraRouter();

// Or map individual event endpoints
// app.MapEntraAttributeCollectionStart();
// app.MapEntraAttributeCollectionSubmit();
// app.MapEntraTokenIssuanceStart();
// app.MapEntraEmailOtpSend();

app.Run();
```

---

## 🧭 Endpoint Mapping Extensions

```csharp
app.MapEntraRouter();                // Multi‑event router
app.MapEntraTokenIssuanceStart();    // Single‑event endpoint
```

### Default Routes

| Endpoint                    | Default Route                |
|-----------------------------|------------------------------|
| Router                      | `router`                     |
| AttributeCollectionStart    | `attributecollectionstart`   |
| AttributeCollectionSubmit   | `attributecollectionsubmit`  |
| TokenIssuanceStart          | `tokenissuancestart`         |
| EmailOtpSend                | `emailotpsend`               |

---

## 🛠 Dependency Injection

```csharp
services.AddEntraEventHandlers();
```

Registers:

- Request/response adapters  
- Handler resolver  
- All `IEntraEventHandler<,>` implementations  
- All ASP.NET Core endpoint classes (router + single‑event)

---

## 🔧 Extensibility

All endpoints inherit from a unified execution pipeline with:

- Overridable logging hooks  
- Centralized exception handling  
- Consistent request/response processing  

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
