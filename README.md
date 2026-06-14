# Project API Clean Architecture

A RESTful API project built with **Clean Architecture** using **.NET 10** and **MySQL**.

The purpose of this project is to demonstrate how Clean Architecture can help developers:

* Improve code maintainability
* Separate business logic from infrastructure concerns
* Reduce coupling between layers
* Simplify testing and debugging
* Minimize bugs when modifying existing features

---

## Architecture Overview

This project follows the Clean Architecture principles by separating responsibilities into different layers:

```text
├── API
│   └── Controllers, Middleware, Dependency Injection
│
├── Application
│   └── Use Cases, DTOs, Interfaces, Business Rules
│
├── Domain
│   └── Entities, Enums, Domain Models
│
├── Infrastructure
│   └── Database, Repository Implementations, External Services
│
└── Persistence
    └── Entity Framework Core, MySQL Configuration
```

### Benefits

* Independent of frameworks
* Easier unit testing
* Better scalability
* Clear separation of concerns
* Easier onboarding for new developers

---

# Table of Contents

* [Getting Started](#getting-started)
* [Prerequisites](#prerequisites)
* [Installation](#installation)
* [Configuration](#configuration)
* [Database Migration](#database-migration)
* [Running the Application](#running-the-application)
* [Project Structure](#project-structure)
* [Technologies](#technologies)

---

# Getting Started

## Prerequisites

Before running the project, make sure you have installed:

* .NET 10 SDK
* MySQL 9.0
* Git

---

## Installation

Clone the repository:

```bash
git clone https://github.com/PaeFaceDev/CleanArchetitureAPI.git
```

Navigate to the project directory:

```bash
cd CleanArchetitureAPI
```

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

---

## Configuration

Update your database connection string in:

```json
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=CleanArchitectureDb;user=root;password=yourpassword;"
  }
}
```

---

## Database Migration

Create migration:

```bash
dotnet ef migrations add InitialCreate
```

Update database:

```bash
dotnet ef database update
```

---

## Running the Application

Start the API:

```bash
dotnet run
```

Or:

```bash
dotnet watch run
```

Swagger UI:

```text
https://localhost:5001/swagger
```

---

## Project Structure

```text
src/
├── API/
│   ├── Controllers/
│   ├── Middleware/
│   └── Program.cs
│
├── Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   └── Features/
│
├── Domain/
│   ├── Entities/
│   ├── Enums/
│   └── Common/
│
├── Infrastructure/
│   ├── Repositories/
│   ├── Services/
│   └── DependencyInjection/
│
└── Persistence/
    ├── Context/
    ├── Configurations/
    └── Migrations/
```

---

## Technologies

* ASP.NET Core 10
* Entity Framework Core
* MySQL 9
* Swagger / OpenAPI
* Clean Architecture
* Dependency Injection
* Repository Pattern

---

## Development Guidelines

### Adding New Features

1. Create Domain Entity
2. Create Application DTOs and Interfaces
3. Implement Use Cases
4. Create Repository Implementation
5. Register Dependencies
6. Expose API Endpoints

### Clean Architecture Rules

* Domain must not depend on other layers.
* Application depends only on Domain.
* Infrastructure depends on Application and Domain.
* API depends on all layers.

---

## Contributing

1. Fork the repository
2. Create a feature branch

```bash
git checkout -b feature/your-feature
```

3. Commit your changes

```bash
git commit -m "Add new feature"
```

4. Push to GitHub

```bash
git push origin feature/your-feature
```

5. Open a Pull Request

---

## License

This project is licensed under the MIT License.
