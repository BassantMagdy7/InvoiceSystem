# InvoiceSystem

A full-stack Invoice Management System built with **ASP.NET Core Web API** backend and **Angular** frontend.

## Tech Stack

**Backend:** ASP.NET Core Web API, Entity Framework Core, JWT, SignalR, SQL Server  
**Frontend:** Angular

## Architecture

```
Controller → Service → Repository → Database
```

- **Controller** — handles HTTP requests/responses and triggers SignalR notifications
- **Service** — business logic and mapping between entities and DTOs
- **Repository** — data access only, always returns entities (no DTOs)

## Getting Started

### Backend

1. `appsettings.Development.json` is excluded from the repository for security reasons.  
   Create it manually in the `WebAPI` folder:



2. Apply EF Core migrations (code first)


3. Run the API:


### Frontend


npm install
ng serve


App runs at `http://localhost:4200`

## Features

- Paginated invoice listing
- Real-time updates via SignalR
- Discount & tax calculation per invoice item