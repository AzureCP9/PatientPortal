# Patient Consultation Portal

A small full-stack take-home that demonstrates a pragmatic patient & consultation workflow:

- Create & list **patients**
- Schedule, view, cancel **consultations**
- Upload & download **attachments**

---

## Tech stack

- **Frontend:** React (TypeScript), Vite + Vike (file-based routing), shadcn/ui (Radix), TanStack Table, date-fns
- **Backend:** .NET 8 Minimal APIs, EF Core, SQL Server
- **Storage:** Azure Blob Storage (local dev: `UseDevelopmentStorage=true`)
- **Tests:** xUnit (domain/units)

---

## Project structure

```
PatientPortal/
├─ PatientPortal.Api/ # .NET 8 Minimal API
├─ PatientPortal.AppHost/ # .NET Aspire Hosting and startup configuration
├─ PatientPortal.Application/ # Application layer (services, DTOs)
├─ PatientPortal.Client/ # Frontend React TS App
├─ PatientPortal.Common/ # Shared utilities and constants
├─ PatientPortal.Domain/ # DDD Domain Models (patients, consultations)
├─ PatientPortal.Domain.Fixtures/ # Domain test data builders
├─ PatientPortal.Domain.UnitTests/ # Domain tests
├─ PatientPortal.Infrastructure/ # Infrastructure (persistence, storage)
├─ PatientPortal.Persistence.Common/ # Persistence abstractions and EF Core entities
└─ PatientPortal.ServiceDefaults/ # Default service configuration and extensions
```

## Assumptions & Deliberate Omissions

This implementation focuses on clarity, maintainability, and showcasing relevant production-style patterns, while keeping scope aligned with the take-home nature of the brief.

- **CQRS dropped for simplicity** – All API endpoints use straightforward application services rather than full CQRS separation.
- **DDD & Clean Architecture influences** – The domain model is built using production-style DDD concepts, immutability where possible, and clear separation between layers.
- **Local-only configuration** – Storage, database, and API configuration are set up for local development only.
  - In production, blob storage configuration and secure access would need further work.
- **No authentication/authorization** – Not included as the brief did not require it.
- **Domain assumptions** – Some domain behaviours (e.g., scheduling rules, cancellation rules) are based on reasonable assumptions given the fictional nature of the brief.
  - In a real project, these would be validated through domain exploration sessions with stakeholders.
- **Unit tests only for core domain** – Key domain behaviours are covered with xUnit tests.
  - Integration tests (with API fixtures and test containers) were skipped due to time constraints but would be included in production, same for frontend tests with such tools as Playwright.
- **Immutability** – The domain model favours immutable design to improve safety and reduce unintended state changes.

Comments are included in places where assumptions were made or where production-level code would require additional work, though they are not extensive again due to time constraints.

## Running locally

### Prerequisites

- .NET 8 SDK
- Node 18+ (or 20+), pnpm/npm/yarn
- Docker (.NET Aspire will handle spinning up all the infra dependencies)

### 1) Database

Update `appsettings.json` (or `appsettings.Development.json`) in `PatientPortal.Api`:

```json
{
  "Persistence": {
    "DesignTimeConnectionString": "Server=localhost,1433;Database=PatientPortalDb;User Id=sa;Password=SuperSecretPassword!1!!;TrustServerCertificate=True;"
  },
  "Storage": {
    "ConnectionString": "UseDevelopmentStorage=true"
  }
}
```

> **Tip:** If you’re running SQL Server in Docker:
>
> ```bash
> docker run --name sql -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SuperSecretPassword!1!!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
> ```

Apply EF Core migrations (if included) or create & update:

```bash
cd PatientPortal.Api
dotnet ef database update
```

### 2) Backend API

```bash
cd PatientPortal.Api
dotnet restore
dotnet run
```

API base URL (default):  
`https://localhost:5001` or `http://localhost:5000`

### 3) Frontend

Configure the API base URL for the web app:

`web/.env.local`

```
PUBLIC_ENV__API_BASE_URL=https://localhost:5001
```

Install & run:

```bash
cd web
pnpm i   # or npm i / yarn
pnpm dev
```

Vite dev server will print the local URL (e.g. `http://localhost:5173`).

---

## What’s implemented

- **Patients**

  - Create patient form (name, age, gender)
  - Patients table (sorting by name, pagination)
  - Edit link

- **Consultations**

  - Create consultation: select patient, date/time, notes, attachments (multi upload)
  - List consultations: sortable by start, status badges, actions (view/cancel)
  - Edit consultation: update schedule (if allowed), notes, attachments
  - **Cancel**: alert-dialog confirmation, disallow cancel after end time

- **Attachments**
  - Uploaded to Azure Blob dev storage; links shown in UI
  - For in-place edit, previously uploaded files are surfaced as read-only list with download links

---

## UX details worth noting

- **Tables**: TanStack Table with numeric accessors for reliable date sorting (stores `Date.getTime()` for sort; formats for UI)
- **Validation UX**:
  - Consistent, compact error messaging (`FormMessage`) with reserved space to avoid layout jump
  - Highlight invalid controls using the theme’s “destructive” color tokens (no hardcoded reds)
- **Form ergonomics**:
  - `fieldset` disables entire form when cancelled
  - When a consultation has started, scheduling inputs are disabled; notes/attachments remain editable
- **Tooltips**: consistent Radix tooltips with provider
- **Confirmations**: destructive actions use `AlertDialog` (no `window.confirm`)

---

## A couple of pragmatic trade-offs

- **Updating past consultations**  
  For the take-home, a single “UpdateDetails” path is used. The domain validation allows editing **notes/attachments** for past consultations while preventing retroactive **schedule changes**. This is done by passing both the proposed schedule and the current schedule into validation; if they’re equal, the “past” check is skipped.

- **Blob access**  
  In dev, blobs are public (via `UseDevelopmentStorage=true`) to avoid SAS/token setup. In production, you’d:
  - make the container private,
  - serve ephemeral SAS URLs from the API,
  - enforce content-type validation server-side.

---

## Domain rules (highlights)

- **Schedule constraints**
  - Duration: 15 min minimum, 2 hours maximum
  - You can’t schedule into the past
  - You can’t change the schedule into the past (but you can update notes/attachments if unchanged)
- **Cancel**
  - Cannot cancel if already cancelled
  - Cannot cancel after `End`

---

## Testing

- **Domain unit tests** (`PatientPortal.Domain.UnitTests`)
  - Focused on schedule validation, state transitions (Scheduled → Cancelled), and status calculations.
  - Run:
    ```bash
    dotnet test
    ```

> Note: xUnit v3 isn’t on NuGet.org at time of writing for general consumption; project uses stable xUnit 2.x runner.

---

## Security & production notes

- Use private blobs + SAS for downloads
- Server-side MIME/type & size validation for uploads
- AuthN/Z (out of scope here)
- Proper error contracts (RFC7807 ProblemDetails) already supported client-side
- Observability: structured logging + tracing

---

## Useful scripts

```bash
# API
cd PatientPortal.Api
dotnet restore && dotnet run

# Web
cd web
pnpm i && pnpm dev

# Tests
dotnet test
```

---

## Known limitations

- No pagination on API endpoints (client paginates)
- No search API
- Attachments list is read-only (no delete)
- No auth (deliberate for demo)
