
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