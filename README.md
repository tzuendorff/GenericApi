# GenericApi

A generic RESTful API built with ASP.NET Core (.NET 8) for managing orders, using MongoDB as the data store. The project demonstrates clean architecture principles, separation of concerns, and includes unit testing with xUnit.

## ToDo
- [ ] Dockerize
- [ ] Add generic Kubernetes manifests with overlays for different staging environments.

## Features

- CRUD operations for `Order` entities via REST endpoints.
- MongoDB integration for persistent storage.
- Swagger/OpenAPI documentation for easy API exploration.
- Configurable database settings via `appsettings.json`.
- Unit tests using xUnit and mock repository.

## Technologies

- ASP.NET Core (.NET 8)
- MongoDB.Driver
- Swashbuckle.AspNetCore (Swagger)
- xUnit (Testing)
- coverlet.collector (Code coverage)

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [MongoDB](https://www.mongodb.com/try/download/community) instance

### Configuration

Set MongoDB connection details in `appsettings.json` under the `MongoDatabase` section:

### Build & Run

The API will be available at `https://localhost:5001` (or as configured).

### API Endpoints

- `POST /orders` - Create a new order
- `GET /orders?orderId={id}` - Retrieve orders (filter by ID substring)
- `PUT /orders` - Update an existing order
- `DELETE /orders?orderId={id}` - Delete an order by ID

Swagger UI is available at `/swagger` in development mode.

## Project Structure

- `src/GenericApi/` - Main API source code
  - `Controllers/` - API controllers
  - `BusinessLogic/` - Business logic layer. Currently empty, but ready for custom logic.
  - `Repositories/` - Data access layer (MongoDB)
  - `Models/` - Data models and configuration classes
- `test/TestGenericApi/` - Unit tests and mock repository

## Extending

- Add new entities by creating models, repositories, and controllers.
- Implement additional business logic by extending the `IBusinessLogic<T>` interface.

## License

This project is provided for educational purposes. Please review and adapt for production use as needed.
