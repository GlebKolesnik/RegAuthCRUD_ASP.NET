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

The project follows a **layered architecture**, where the main logic is separated into different layers.

### Design Patterns

1. **Repository Pattern**:
   - The **Repository** pattern is used to encapsulate data access logic. Each entity (like `Task` or `User`) has its own repository interface and implementation. 

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

## How to Test the API

### API testing examples will be shown using Postman.

**You can also create two users, and on behalf of each of them, create multiple tasks.
And then try, for example, to request a task by its id, and take the id of another user's task. After you execute the request, you will get an empty body, because users do not have access to other users' tasks.**

1. **Registration**
   - **Type**: `POST`
   - **URL**: `http://localhost:8080/api/users/register`
   - **Request Body**:
   ```json
   {
     "username": "testuser",
     "email": "testuser@gmail.com",
     "password": "Moloko_1"
   }
2. **Authentication**
- **Type**: `POST`
- **URL**: `http://localhost:8080/api/users/login`
- **After successful authentication, you will receive a token in the following format:**
```json
{
  "username": "testuser",
  "email": "testuser@gmail.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1NjUzMDU1NS1kNzBjLTRiOTctOGQzYS05NTU5YTI1NDViZWYiLCJ1bmlxdWVfbmFtZSI6ImdvbGVyMzM4IiwiZW1haWwiOiJnb2xlcm92aWNoQGdtYWlsLmNvbSIsIm5iZiI6MTcyNTYzNDc1NywiZXhwIjoxNzI1NjM4MzU3LCJpYXQiOjE3MjU2MzQ3NTcsImlzcyI6IlRlc3RBc3NpZ21lbnRfSEsiLCJhdWQiOiJUZXN0QXNzaWdtZW50X0hLIn0.HqeZyQ6iMeb7OxmWmxVFs3K6xIqXICEvqK4rj_Z30Ik"
}
```
Copy the entire content of the ```token``` field and use it in the next request.
- **Request Body**:
```json
{
    "emailOrUsername": "testuser",
    "password": "Moloko_1"
}
```
3. **Create Task**
- **Type**: `POST`
- **URL**: `http://localhost:8080/api/tasks`
- **Header**: In 'Key' field: `Authorization`, In 'Value' field `Bearer <token>`
- **Body**: 
```json
{
    "title": "Test Task",
    "description": "This is a test task",
    "dueDate": "2024-09-10T10:00:00",
    "priority": 1,  
    "status": 0    
}
```

4. **GetListTasks**
- **Type**: `GET`
- **URL**: `http://localhost:8080/api/tasks`
- **Header**: In 'Key' field: `Authorization`, In 'Value' field `Bearer <token>`
- **Request Body**: `empty`

5. **GetTaskByID**
- **Type**: `GET`
- **URL**: `http://localhost:8080/api/tasks/{id}`
- **Header**: In 'Key' field: `Authorization`, In 'Value' field `Bearer <token>`
- **Request Body**: `empty`
  
6. **EditTaskByID**
- **Type**: `PUT`
- **URL**: `http://localhost:8080/api/tasks/{id}`
- **Header**: In 'Key' field: `Authorization`, In 'Value' field `Bearer <token>`
- **Request Body**: 
```json
{
    "title": "Updated Task Title",
    "description": "Updated Task Description",
    "dueDate": "2024-10-01T10:00:00",
    "priority": 2,  
    "status": 1     
}
```
7. **DeleteTaskById**
- **Type**: `DELETE`
- **URL**: `http://localhost:8080/api/tasks/{id}`
- **Header**: In 'Key' field: `Authorization`, In 'Value' field `Bearer <token>`
- **Request Body**: `empty`
