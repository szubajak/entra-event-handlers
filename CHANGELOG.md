# Changelog

All notable changes to this project's [**Entra.EventHandlers.Abstractions**][nuget] package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project attempts to adhere to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## v1.5.3 — 2026‑09‑22

### Changed

- Changed `DecryptAsync` to support a `CancellationToken` parameter.

## v1.5.2 — 2026‑09‑22

### Changed

- Changed `IPasswordContextDecryptor` to use an asynchronous `DecryptAsync` method.
  This enables non‑blocking retrieval of private keys from external providers such as Azure Key Vault.

## v1.5.1 — 2026‑09‑22

### Added

- Added `IPasswordContextDecryptor` as the new extension point for decrypting the encryptedPasswordContext field in `PasswordSubmitEvent`.
- Added `DecryptedPasswordContext` model, representing the decrypted password, nonce, and optional username.

## v1.5.0 — 2026‑09‑15

### Changed

- **Breaking:** `IEntraEventHandler` method `HandleAsync` now return `EntraHandlerResult<TResponse>` instead of raw `TResponse`. 
  Existing implementations require minimal updates (typically wrapping the response).

### Added

- Added `EntraHandlerResult<TResponse>` as the unified result wrapper for all handler executions.
- Added `ExceptionExtensions` with exception classification and error‑code mapping.

### Removed

- Removed `EntraErrorResponse` from the abstractions package.
  Error response shaping is now handled exclusively by hosting‑specific packages

## v1.4.2 – 2026‑07‑16

### Updated

- Updated NuGet package description for clarity and consistency. No API or behavioral changes.

## v1.4.1 — 2026‑06‑26

### Changed

- Made EntraEventResponse<TPayload>.Data a required property to ensure all response payloads are always populated.

## v1.4.0 — 2026‑06‑24

### Changed

- **Breaking:** `IEntraEventHandler` method `Handle` has been renamed to `HandleAsync`. 
  All implementations must update their method signature.

### Added

- Added `VerifiedIdClaimValidationEvent` request model.
- Added `VerifiedIdClaimValidationEventPayload` including verified ID claims context support.
- Added `VerifiedIdClaimValidationPassAction` and `VerifiedIdClaimValidationFailedAction`
- Added `IVerifiedIdClaimValidationHandler` for handling VerifiedIdClaimValidation events.

## v1.3.3 — 2026‑06‑18

### Fixed

- Added missing `PasswordSubmit` discriminator to `EntraEvent` to enable correct deserialization of `PasswordSubmitEvent`.

## v1.3.2 — 2026‑06‑10

### Updated

- Updated README.md

## v1.3.1 — 2026‑06‑10

### Updated

- Updated XML documentation.
- Seal contract classes.

## v1.3.0 — 2026‑06‑09

### Updated

- Updated NuGet metadata.
- Updated XML documentation.

### Added

- Added `PasswordSubmitEvent` request model.
- Added `PasswordSubmitEventPayload` including encrypted password context support.
- Added `PasswordSubmitAction` and `PasswordSubmitActionType`
- Added `IPasswordSubmitHandler` for handling PasswordSubmit events.

## v1.2.4 — 2026‑06‑05

### Modified

- Modified `EntraDeserializationException`.

## v1.2.4 — 2026‑06‑05

### Added

#### Exception Types
- Added `EntraDeserializationException`, `EntraHandlerNotFoundException` and `EntraValidationException` exceptions.
- Added `EntraErrorCodes` and `EntraErrorResponse`.

## v1.2.1 — 2026‑06‑03

### Modified

- Improved README.md and LICENSE.

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