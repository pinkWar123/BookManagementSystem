# Book Management System Backend

This repository contains the backend code for the **Book Management System**. The system is built using .NET Core and follows the MVC architecture pattern with three layers: **Controller**, **Service**, and **Repository**. The backend provides RESTful APIs for managing books, authors, and other related entities.

## Features

- Manage Books: Create, Read, Update, and Delete (CRUD) operations for books.
- Manage Authors: CRUD operations for author entities.
- User Authentication & Authorization (Optional feature).
- Book searching and filtering capabilities.
- Layered architecture (Controller, Service, Repository) for separation of concerns.

## Technologies Used

- **.NET Core 6.0** - Core framework for building the backend.
- **ASP.NET MVC** - Used for routing and API controller handling.
- **Entity Framework Core** - For data persistence and ORM (Object-Relational Mapping).
- **SQL Server** - Database engine used for data storage (can be switched with other DB providers).
- **Swagger** - For API documentation and testing.
- **AutoMapper** - For object-to-object mapping.

## Architecture

This project follows a **3-layer architecture**:

### Controllers

The **Controllers** are responsible for handling HTTP requests and sending responses. Each controller is associated with a specific domain entity (e.g., BooksController, AuthorsController). Controllers do not contain business logic. Instead, they delegate the work to the service layer.

- Location: `Api/Controllers`

### Services

The **Service Layer** contains the business logic of the application. Services fetch, manipulate, and validate data before sending it to the repository or returning it to the controllers.

- Location: `Application/Services`
- Example services: `BookService`, `AuthorService`.

### Repositories

The **Repository Layer** handles the data persistence. It interacts with the database through Entity Framework and provides CRUD operations to the service layer.

- Location: `Infrastructure/Repositories`
- Example repositories: `BookRepository`.

## Getting Started

### Prerequisites

Ensure you have the following installed:

- **.NET 6.0 SDK** or later.
- **SQL Server** (or any supported database).
- **Visual Studio** (or Visual Studio Code).

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/BookManagementSystemBackend.git
   ```
2. Restore dependencies:
    ```bash
   dotnet restore
   ```
### Database
1. Update the appsettings.json with your SQL Server connection string:
    ```bash
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=BookManagementDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
    }
    ```
2. Apply migrations to set up the database:
   ```bash
   dotnet ef database update
   ```
 ### Running the Application
 1. To run the project, use the following command:
    ```bash
    dotnet run
    ```
2. The application will be available at https://localhost:5001 (or the configured port).

3. Swagger documentation will be available at https://localhost:5001/swagger.
## API Endpoints
Here are some of the key API endpoints provided by this backend:
### Books

- GET /api/books - Get a list of all books.
- GET /api/books/{id} - Get details of a single book by ID.
- POST /api/books - Add a new book.
- PUT /api/books/{id} - Update an existing book.
- DELETE /api/books/{id} - Delete a book.
### Contributing
1. Contributions are welcome! If you'd like to contribute, please follow these steps:
2. Fork the repository.
3. Create a new branch (git checkout -b feature-branch).
4. Commit your changes (git commit -m 'Add some feature').
5. Push to the branch (git push origin feature-branch).
6. Open a pull request.

   
   
   
