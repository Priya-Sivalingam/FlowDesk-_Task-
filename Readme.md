
A backend REST API service powering the FlowDesk Task Board — a smart workspace platform for remote teams. Built with ASP.NET Core 8, Entity Framework Core, and PostgreSQL.
---

## Features

- JWT-based authentication with role support (Admin / Member)
- Auto-generated Employee IDs in format `FD-YYYY-XXXX`
- Project management with member roles (ProjectManager / TeamLead / Member)
- Task creation, assignment, priority, and due date management
- Task workflow: ToDo → InProgress → Done / Cancelled
- Task archiving without permanent deletion
- Filter tasks by status, priority, or assignee

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 |
| Language | C# 12 |
| ORM | Entity Framework Core 8 |
| Database | PostgreSQL |
| Auth | JWT Bearer Tokens |
| Validation | FluentValidation |
| Logging | Serilog |
| API Docs | Swagger / Swashbuckle |
| Password Hashing | BCrypt.Net |

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (local or remote)
- [Git](https://git-scm.com/)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Priya-Sivalingam/FlowDesk-_Task-
cd FlowDesk-_Task-
```

### 2. Configure the Database

Create `appsettings.json` and fill in your PostgreSQL details:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=YOUR_HOST;Port=YOUR_PORT;Database=flowdesk;Username=YOUR_USER;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "Key": "your-secret-key-minimum-32-characters",
    "Issuer": "FlowDesk",
    "Audience": "FlowDeskUsers",
    "ExpiryHours": 8
  }
}
```

### 3. Install Dependencies

```bash
dotnet restore
```

### 4. Run Database Migrations

```bash
dotnet ef database update
```

This will automatically create all tables in your PostgreSQL database.

### 5. Run the Application

```bash
dotnet run
```

### 6. Open Swagger UI

```
http://localhost:5287/swagger

## Postman Collection

Import both files into Postman to explore the API:

- `FlowDesk.postman_collection.json` — all endpoints
- `FlowDesk.postman_environment.json` — environment variables



## Design Decisions

**Repository Pattern**
Separates data access from business logic. Makes the codebase testable and allows swapping the database without touching services or controllers.

**Global Exception Middleware**
Centralised error handling maps typed exceptions to HTTP status codes consistently across the entire API surface, removing try/catch from every controller.

**DTOs over Entities**
Entities are never exposed directly. DTOs control exactly what data enters and leaves the API — preventing over-posting and hiding sensitive fields like `PasswordHash`.

**FluentValidation**
Validation rules are defined in dedicated validator classes rather than data annotations, keeping entities clean and making rules easy to test independently.

**Typed Exceptions**
Services throw specific exception types (`KeyNotFoundException`, `ArgumentException`, `InvalidOperationException`) which the middleware maps to the correct HTTP status codes automatically.

**BCrypt Password Hashing**
Passwords are never stored in plain text. BCrypt adds a salt automatically and is resistant to brute-force attacks.
