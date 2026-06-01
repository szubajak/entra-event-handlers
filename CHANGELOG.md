# Changelog

All notable changes to this project's [**Entra.EventHandlers.Abstractions**][nuget] package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project attempts to adhere to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## v1.1.0 — 2026‑06‑01

### Added

#### Core Event Models
- Added `EmailOtpSendEvent` request model.
- Added `EmailOtpSendEventPayload` with full protocol‑accurate OTP context mapping.

#### Response Models
- Added `EmailOtpSendResponse` and `EmailOtpSendResponsePayload`.

#### Handler Interfaces
- Added `IEmailOtpSendHandler` for processing EmailOtpSend events.


## v1.0.0 — 2026‑05‑28

### Added

#### Core Event Models
- Added `AttributeCollectionStartEvent` and `AttributeCollectionStartEventPayload` request model.
- Added `AttributeCollectionSubmitEvent` and `AttributeCollectionSubmitEventPayload` request model.
- Added `TokenIssuanceStartEvent` and `TokenIssuanceStartEventPayload` request model.

#### Response Models
- Added `AttributeCollectionStartResponse`.
- Added `AttributeCollectionSubmitResponse`.
- Added `TokenIssuanceStartResponse`.
- Added base `EntraEventResponse` type.

#### Action Types
- Added `ContinueAction` with `ContinueActionType`.
- Added `ShowBlockPageAction` with `ShowBlockPageActionType`.
- Added `SetPrefillValuesAction`.
- Added `ShowValidationErrorAction`.
- Added `ModifyAttributeValuesAction`
- Added base `EntraAction` type.

#### Protocol Constants
- Added `EntraEventTypes` for event identifiers.
- Added `EntraOdataTypes` for all OData types identifiers.
- Added `DirectoryAttributeTypes` for attribute classification.

---