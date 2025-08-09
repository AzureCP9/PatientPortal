# Patient Portal - Take-Home Demo

## Introduction

A small full-stack take-home app demonstrating a pragmatic patient & consultation workflow:

- Create & list **patients**
- Schedule, view, cancel **consultations**
- Upload & download **attachments** within a consultation

---

## Getting Started

**Prerequisites**

- .NET 8 SDK
- Node 18+ (or 20+)
- Docker (Aspire will spin up dependencies)

**Steps**

1. Ensure Docker is running.
2. From solution root:
   ```sh
   dotnet run --project ./PatientPortal.AppHost
   ```
3. From `./PatientPortal.Client`:
   ```sh
   pnpm install
   pnpm dev
   ```
4. Open the browser at the provided URL.

---

## Tech stack

**Frontend**

- React (TypeScript)
- Vite + Vike (file-based routing)
- shadcn/ui (Radix)
- TanStack Table
- date-fns

**Backend**

- .NET 8 Minimal APIs
- EF Core, SQL Server
- Azure Blob Storage (local dev: `UseDevelopmentStorage=true`)

**Testing**

- xUnit (domain/unit tests only for brevity)

---

## Project structure

```
PatientPortal/
├─ PatientPortal.Api/                 # Minimal API endpoints
├─ PatientPortal.AppHost/             # Aspire hosting & startup
├─ PatientPortal.Application/         # Application services, DTOs
├─ PatientPortal.Client/              # React frontend
├─ PatientPortal.Common/              # Shared utilities/constants
├─ PatientPortal.Domain/              # DDD domain models
├─ PatientPortal.Domain.Fixtures/     # Domain test data builders
├─ PatientPortal.Domain.UnitTests/    # Domain tests
├─ PatientPortal.Infrastructure/      # Infra (persistence, storage)
├─ PatientPortal.Persistence.Common/  # Persistence abstractions
└─ PatientPortal.ServiceDefaults/     # Service config/extensions
```

---

## Design notes

This implementation prioritises **clarity** and **maintainability** over full production scope.

- **No CQRS** – All endpoints call application services directly (simpler for a take-home).
- **DDD influences** – Domain models are immutable where possible, clear layer separation.
- **Local-only config** – All services run locally; production setup (auth, secure blob access, etc.) is out of scope.
- **Domain rules** – Scheduling & cancellation rules reflect reasonable assumptions for a fictional brief.
- **Testing** – Focused on domain unit tests. No API/frontend integration tests to save time.

---

## UX highlights

- **Tables** – TanStack Table with numeric date sorting (`getTime()` for sort, formatted in UI)
- **Validation** – Compact error messages, reserved space to prevent layout shift
- **Forms** – `fieldset` disables full form when cancelled; past consultations lock scheduling inputs
- **Tooltips** – Consistent Radix implementation
- **Confirmations** – Destructive actions use `AlertDialog`

---

## Pragmatic trade-offs

- **Updating past consultations** – Notes/attachments can be edited; schedule changes are blocked if in the past, instead of splitting commands CQRS-style
- **Blob access** – Dev mode uses public blobs; prod would use private containers with SAS URLs.
- **Frontend component reuse** – Some UI logic and components is in-file; would extract and refactor in production for maintainability.

---

## Known limitations

- No API pagination (client handles paging)
- No search API
- Attachment list is read-only
- No authentication/authorization
