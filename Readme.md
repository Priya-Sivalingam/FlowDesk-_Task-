
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
cd FlowDesk.Api
```

### 2. Configure the Database

Create `appsettings.json` and fill in your PostgreSQL details:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=flowdesk;Username=postgres;Password=yourpassword"
  },
  "Jwt": {
    "Key": "your-super-secret-key-minimum-32-characters",
    "Issuer": "FlowDesk",
    "Audience": "FlowDeskUsers",
    "ExpiryHours": 8
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
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