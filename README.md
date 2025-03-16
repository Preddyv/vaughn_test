# Developer Test: Build a Web Application with API Integration

## Objective

Create a web application with a backend and frontend that fetches and displays user data from an external service.

## Requirements

### Backend (C# .NET Core)
- Build a backend using .NET Core in VS Code.
- Expose an endpoint to retrieve a list of users from https://jsonplaceholder.typicode.com/users.
- Fetch the data from this URL and return it in a suitable format.
- Implement functionality to modify or delete users in memory or a storage solution.
- (New Requirement) Implement an API endpoint that finds the nearest user to each hotel based on latitude and longitude.

### Frontend (Vue 3 with TypeScript)
- Create a frontend using Vue 3 with TypeScript in VS Code.
- Fetch user data from the backend and display it in a simple list.
- Implement functionality to add new users and handle user interactions.
- Structure the frontend in a modular way, separating logic into components.

### Bonus Tasks
- Implement API calls using a common HTTP library (e.g., Axios).
- Handle loading and error states in the UI.
- Apply basic CSS styling to the UI.
- Use Pinia for state management (extra points for this).

### Unit Test Requirement
- Write unit tests to calculate the nearest user to each hotel based on latitude and longitude.
- The following hotels should be used for testing:
  - Hotel A: (-43.9509, -34.4618)
  - Hotel B: (40.7128, -74.0060)
  - Hotel C: (34.0522, -118.2437)
  - Hotel D: (-25.2744, 133.7751)

### Test Cases
The test suite includes the following test cases:

1. GetNearestUsersAsync
   - Verifies that the correct nearest user is found for each hotel
   - Tests with a dataset of 5 users with different geographical locations
   - Validates that each hotel is matched with its closest user based on coordinates

2. AddUserAsync
   - Tests the ability to add a new user to the system
   - Verifies that the new user is assigned a valid ID
   - Confirms the user is properly added to the collection

3. DeleteUserAsync
   - Tests successful deletion of an existing user
   - Verifies handling of deletion attempts for non-existent users
   - Confirms the user count is updated after deletion

Each test uses a mock HTTP client to simulate API calls and includes test data representing real-world scenarios.

## Deliverables

A repository containing:
- A backend project in .NET Core
- A frontend project in Vue 3 with TypeScript
- A README file with setup instructions

## Evaluation Criteria
- Code structure and clarity
- Proper API integration and functionality
- UI component design and user interaction handling
- Error handling and readability
- Use of Pinia for state management (extra points)
- Correct implementation of nearest user calculation with unit tests

## Setup Instructions

### Backend

1. Install .NET Core SDK from https://dotnet.microsoft.com/download.
2. Open the `backend` folder in VS Code.
3. Restore the dependencies by running the following command in the terminal:
   ```
   dotnet restore
   ```
4. Build the project:
   ```
   dotnet build
   ```
5. Run the project:
   ```
   dotnet run --project Backend/Backend.csproj
   ```
   The API will be available at `https://localhost:7001` or `http://localhost:5290`

### Frontend

1. Install Node.js from https://nodejs.org/.
2. Open the `frontend` folder in VS Code.
3. Install the dependencies by running the following command in the terminal:
   ```
   npm install
   ```
4. Run the project:
   ```
   npm run dev
   ```
   The frontend will be available at `http://localhost:5173`

## Running Unit Tests

### Backend

1. Open the `backend` folder in VS Code.
2. Run the unit tests:
   ```
   dotnet test Tests/Tests.csproj
   ```

### Frontend

1. Open the `frontend` folder in VS Code.
2. Run the unit tests:
   ```
   npm run test:unit
   ```

## Development Notes

- The repository has a `.gitignore` file that excludes build artifacts, binary files, and other non-source code files.
- Only commit source code files, configuration files, and documentation to the repository.
