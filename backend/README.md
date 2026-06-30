# Invoice System Backend

ASP.NET Core Web API backend for an invoice management system.

## Features

- User registration and login
- ASP.NET Core Identity
- JWT authentication using HttpOnly cookie
- Protected invoice APIs
- Create, read, update and delete invoices
- Invoice items with quantity, price, discount and tax
- Automatic invoice total calculation
- Store, customer and product lookup APIs
- Invoice list ordered by newest first
- Pagination for invoice list
- SignalR real-time invoice refresh
- Swagger API documentation
- EF Core migrations
- Seed data for testing

## Technologies

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server LocalDB
- ASP.NET Core Identity
- JWT Bearer Authentication
- SignalR
- Swagger / OpenAPI

## NuGet Packages

Main backend packages used:


Microsoft.AspNetCore.Identity
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.AspNetCore.Authentication.JwtBearer