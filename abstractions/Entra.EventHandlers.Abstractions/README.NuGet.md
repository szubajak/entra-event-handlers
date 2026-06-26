# Entra.EventHandlers.Abstractions

Lightweight, dependency‑free **public abstractions** for building  
**Microsoft Entra External ID and Workforce Authentication Event Handlers**.

This package is MIT‑licensed and safe to use in all scenarios, including commercial applications.

---

## ✨ Features

### ✔ Strongly‑Typed Protocol Models

Includes complete request/response models for:

- AttributeCollectionStart  
- AttributeCollectionSubmit  
- EmailOtpSend  
- PasswordSubmit  
- TokenIssuanceStart  
- VerifiedIdClaimValidation  

Each event includes:

- Request model  
- Response model  
- Action definitions  
- Payload types  
- Handler interface  

---

### ✔ Handler Interfaces

Defines the core handler contracts used across the ecosystem:

```csharp
public interface IAttributeCollectionStartHandler
    : IEntraEventHandler<AttributeCollectionStartEvent, AttributeCollectionStartResponse> { }
```

All interfaces are fully XML‑documented for a first‑class developer experience.

---

### ✔ Framework‑Agnostic

The abstractions package contains **no hosting logic** and **no dependencies**.

Use it freely with:

- ASP.NET Core  
- Azure Functions  
- Custom hosts  
- Unit tests  
- Any DI container  

---

## 📦 Related Packages

- **Entra.EventHandlers** — implementation layer with builders & base handlers (BSL)  
- **Entra.EventHandlers.Workforce** — Workforce‑specific models & builders (BSL)  
- **Entra.EventHandlers.AspNetCore** — ASP.NET Core hosting adapter (BSL)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions hosting adapter (BSL)

---

## 🔒 License

This package is licensed under the **MIT License**.

See:
- `LICENSE` — full MIT terms  

The implementation and hosting adapters are available under the  
Business Source License (BSL) in the related packages.

---

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID Authentication Event Handlers,
Workforce scenarios, and the design of this ecosystem, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**  
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
