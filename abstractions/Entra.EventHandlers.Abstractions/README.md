# Entra.EventHandlers.Abstractions

**License:** MIT  
**Author:** Jakub Szubarga (Szubarga.NET)  
This package is free for all use cases, including commercial use.

Lightweight, dependency‑free **public abstractions** for building  
**Microsoft Entra External ID and Workforce Authentication Event Handlers**.

This package defines the protocol‑level contract for the entire ecosystem:
event request/response models, action definitions, directory attribute primitives,
and handler interfaces. All types include full XML documentation for a first‑class
developer experience.

---

## ✨ What This Package Provides

The abstractions package contains the **public contract** used by all Entra Event Handler extensions:

- Strongly‑typed **event request models**  
- Strongly‑typed **response models**  
- **Action definitions** and protocol constants  
- **Directory attribute primitives**  
- **Handler interfaces**, e.g.:  
```csharp
public interface IAttributeCollectionStartHandler
    : IEntraEventHandler<AttributeCollectionStartEvent, AttributeCollectionStartResponse> { }
```
- Enums and metadata types  
- OData‑typed payload models  
- Event type identifiers  

These types represent the JSON protocol used by Microsoft Entra External ID and Workforce authentication flows.

The abstractions are stable, versioned, and safe to depend on in long‑term projects.

---

## 🧩 Supported Events

This package includes complete request/response models and handler interfaces for all currently supported events:

### **External ID Events**
- AttributeCollectionStart  
- AttributeCollectionSubmit  
- EmailOtpSend  
- PasswordSubmit (just‑in‑time password migration)  
- TokenIssuanceStart  

### **Workforce Event**
- VerifiedIdClaimValidation (account recovery)

Each event includes:

- Request model  
- Response model  
- Payload types  
- Action definitions  
- Handler interface  

---

## 🧩 Why a Separate Abstractions Package?

The abstractions are **MIT‑licensed** to maximize adoption and interoperability.

They allow you to:

- Build your own handlers  
- Integrate with Entra External ID or Workforce events  
- Test locally without Azure  
- Reference the protocol without pulling in implementation details  
- Use the models in any hosting environment (Functions, ASP.NET Core, custom hosts)

The full implementation lives in separate packages under the Business Source License (BSL).

---

## 📦 Related Packages

These packages extend the abstractions with production‑ready functionality:

### **Entra.EventHandlers** — implementation layer (BSL)
- Fluent response builders  
- Handler base classes  
- Validation  
- Logging & telemetry  

### **Entra.EventHandlers.Workforce** — Workforce event models & builders (BSL)
- VerifiedIdClaimValidation response builders  
- Workforce handler base classes  

### **Entra.EventHandlers.AspNetCore** — ASP.NET Core adapter (BSL)
- Minimal API endpoint integration  
- Router endpoint  
- Single‑event endpoint classes  

### **Entra.EventHandlers.AzureFunctions** — Azure Functions adapter (BSL)
- Automatic request/response handling  
- DI wiring  
- Minimal boilerplate for production deployments  

All packages are available on NuGet.

---

## 📄 License

This package is licensed under the **MIT License**.  
See the [LICENSE](./LICENSE) file for details.

The implementation and hosting adapters are available under the  
**Business Source License (BSL)** in the related packages.

---

## 📚 Documentation

Full documentation, examples, and production templates are available in the main repository.

---

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID Authentication Event Handlers  
and the design of this ecosystem, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**  
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
