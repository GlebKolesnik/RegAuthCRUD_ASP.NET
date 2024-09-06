# RegAuthCRUD_ASP.NET

## Getting Started with Docker

To install and run the project using Docker, follow these steps:

1. Clone or download the project files to your local machine.
   
2. Open a terminal (CMD or PowerShell) and navigate to the directory where you saved the project files.

3. Run the following command to build and start the containers:
   ```bash
   docker-compose up --build
4. The project will be built and two containers will be started.

5. If you want to stop and remove the containers (along with their volumes), run the following command:
   ```bash
   docker-compose down --volumes
## Project Architecture Overview

The project follows a **layered architecture**, where the main logic is separated into different layers for better maintainability and scalability.

### Design Patterns

1. **Repository Pattern**:
   - The **Repository** pattern is used to encapsulate data access logic. Each entity (like `Task` or `User`) has its own repository interface and implementation. This allows for:
     - Separation of concerns: The business logic doesn't directly deal with database queries.
     - Better testability: Since repositories can be easily mocked, it becomes easier to write unit tests.
     - Centralized data access logic: All the database-related logic is contained in a repository, making it easier to maintain.

   Example:
   - `ITaskRepository` and `TaskRepository` handle operations related to `Task` entities (e.g., `AddTaskAsync`, `GetTaskByIdAsync`).
   - `IUserRepository` and `UserRepository` handle operations related to `User` entities (e.g., `AuthenticateUserAsync`, `RegisterUserAsync`).

2. **Service Layer**:
   - The **Service Layer** acts as an intermediary between the controllers and the repositories. It contains the business logic and ensures that the controllers don't directly access the database. 
   - This layer is where validation, business rules, and application logic are implemented.

   Example:
   - `TaskService` orchestrates the creation, updating, and retrieval of tasks.
   - `UserService` handles user authentication and registration.

3. **DTOs (Data Transfer Objects)**:
   - DTOs are used to transfer data between layers (e.g., between the client and the controller or between the controller and the service). 
   - They help in ensuring that only the necessary data is passed around, thus improving performance and security.

   Example:
   - `RegisterUserDTO` is used to register a new user, containing fields like `Username`, `Email`, and `Password`.
   - `TaskDTO` represents a task with fields like `Title`, `Description`, `DueDate`, `Priority`, and `Status`.

4. **Controller Layer (API)**:
   - The **Controller Layer** is the entry point for client requests (HTTP requests). Each controller corresponds to a specific entity and handles incoming requests, such as creating, reading, updating, and deleting data.
   - The controller interacts with the service layer to handle the business logic and returns the result to the client.

   Example:
   - `TasksController` manages all task-related API endpoints (`GET`, `POST`, `PUT`, `DELETE`).
   - `UsersController` manages user authentication and registration endpoints.

### Why Use Repositories and Services?

- **Repositories**: 
  - Repositories abstract the data layer and allow for easy changes to the underlying data storage mechanism (e.g., switching from SQL to NoSQL). They also improve code organization by keeping the data access logic separate from the business logic.

- **Services**: 
  - The service layer provides a clear boundary between the application’s business logic and the data access logic. It allows for better organization of business rules and improves testability by decoupling the business logic from the data access layer.

### Key Methods:

- **TaskService**:
  - `AddTaskAsync(TaskDTO taskDto, Guid userId)`: Adds a new task for the user.
  - `GetTaskByIdAsync(Guid id, Guid userId)`: Retrieves a task by its ID and checks if the user is authorized to access it.
  - `UpdateTaskAsync(Guid id, TaskDTO taskDto, Guid userId)`: Updates an existing task's details.
  - `DeleteTaskAsync(Guid id, Guid userId)`: Deletes a task if the user is authorized.

- **UserService**:
  - `AuthenticateUserAsync(string emailOrUsername, string password)`: Authenticates a user using their email or username and password.
  - `RegisterUserAsync(RegisterUserDTO userDto)`: Registers a new user and returns the result of the operation.

---

By using a layered architecture along with the repository and service patterns, the project achieves clean separation of concerns, easier maintainability, and better testability. Each component has a clear responsibility, which allows for easy scaling and modifications in the future.

