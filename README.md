# FinTrack

A personal finance tracking application with a C# backend and React frontend.

## Features

- **User Authentication** - Register, login, JWT-based sessions
- **Accounts** - Manage bank accounts, credit cards, and wallets with balances
- **Transactions** - Track income, expenses, and transfers between accounts
- **Categories** - Organize transactions with hierarchical categories (custom icons and colors)
- **Budgets** - Set weekly, monthly, or yearly spending limits per category
- **API Documentation** - Swagger UI available at `/swagger`

## Tech Stack

### Backend
- ASP.NET Core 8.0 (Clean Architecture)
- PostgreSQL 16
- Entity Framework Core + Npgsql
- MediatR (CQRS)
- FluentValidation
- JWT Bearer Authentication
- BCrypt password hashing
- xUnit for testing

### Frontend
- React 19 + TypeScript
- Vite
- React Router v7
- Axios
- Tailwind CSS

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/)
- [Docker](https://www.docker.com/)

## Installation & Setup

### 1. Start the database

```bash
docker-compose up -d
```

This starts a PostgreSQL 16 container on port 5432.

### 2. Run the backend

```bash
cd FinTrackPro-Backend
dotnet restore
dotnet build
dotnet run --project FinTrackPro.API
```

The API runs at `http://localhost:5119`. Swagger docs are at `http://localhost:5119/swagger`.

To apply database migrations:

```bash
dotnet ef database update --project FinTrackPro.Infrastructure --startup-project FinTrackPro.API
```

### 3. Run the frontend

```bash
cd FinTrackPro-Frontend
npm install
npm run dev
```

The frontend runs at `http://localhost:5173`.

## Project Structure

```
FinTrack-Pro/
├── docker-compose.yml
├── FinTrackPro-Backend/
│   ├── FinTrackPro.API/            # Entry point, controllers, middleware
│   ├── FinTrackPro.Application/    # Use cases, CQRS commands/queries
│   ├── FinTrackPro.Domain/         # Entities, value objects, enums
│   ├── FinTrackPro.Infrastructure/ # EF Core, database configs, migrations
│   └── FinTrackPro.Tests/          # xUnit tests
└── FinTrackPro-Frontend/
    └── src/
        ├── api/          # Axios API clients
        ├── components/   # Reusable UI components
        ├── contexts/     # React contexts (auth)
        ├── pages/        # Route pages
        └── types/        # TypeScript type definitions
```

## Future

- **Recurring Transactions** - Domain model exists for scheduling repeating income/expenses, not yet wired up to the API or UI
- **Token Refresh** - Endpoint exists but not yet implemented
