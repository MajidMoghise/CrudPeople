# CrudPeople

This project is designed for managing and storing people’s information in a database. It supports two database types: **MongoDB** and **SQL Server**, offering flexibility based on the configuration. The project has been carefully structured to ensure reliability, scalability, and clean architecture.

---

## Key Features

### Database:
- **MongoDB and SQL Server**:
  - The project supports either MongoDB or SQL Server, determined via configuration.
  - The application cannot use both databases simultaneously.

### Architecture and Design:
- **CQRS for SQL Server**:
  - The Command and Query Responsibility Segregation (CQRS) pattern is used for SQL Server, separating write and read operations for better scalability and maintainability.
- **Repository Pattern**:
  - Both MongoDB and SQL Server implementations leverage the Repository pattern for abstracting data access logic.
- **Unit of Work**:
  - Transactions and database operations are managed centrally using the Unit of Work pattern.

### Error Handling:
- **Middleware**:
  - A central middleware handles application errors and ensures consistent error responses.

### Concurrency:
- **RowVersion**:
  - RowVersion is used to prevent data overlap issues and ensure that concurrent updates by multiple users do not cause conflicts.

---

## Configuration
To select the database used by the application, modify the `appsettings.json` file. An example configuration:

```json
{
  "DataBaseWorking": {
    "MongoDb": true,
    "Sql": false
  }
}
