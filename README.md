# FakeRestAPI Test Automation

API test automation framework for https://fakerestapi.azurewebsites.net

## Tech Stack
- .NET 9.0
- xUnit
- FluentAssertions
- Newtonsoft.Json

## Project Structure
```
FakeRestApiTests/
├── Helpers/
│   ├── ApiClient.cs          # HTTP client wrapper for API calls
│   └── ApiTestLogger.cs      # Logger for test failures
├── Models/
│   ├── Activity.cs           # Activity model
│   ├── Author.cs             # Author model
│   └── Book.cs               # Book model
└── Tests/
    ├── ActivitiesTests.cs    # Tests for Activities endpoint
    ├── AuthorsTests.cs       # Tests for Authors endpoints
    └── BooksTests.cs         # Tests for Books endpoints
```

## Test Coverage

### 1. Activities Tests
- **GET /api/v1/Activities**
  - Verifies successful response (status code 200)
  - Verifies the number of activities equals 30
  - Verifies no activity has a due date of yesterday

### 2. Authors Tests
- **POST /api/v1/Authors**
  - Verifies successful response
  - Verifies response body matches request body
- **DELETE /api/v1/Authors/{id}**
  - Verifies successful deletion of created author

### 3. Books Tests
- **PUT /api/v1/Books/{id}**
  - Verifies successful response
  - Verifies response body matches request body
- **GET /api/v1/Books/{id}** (for ids 1-10)
  - Verifies successful response
  - Verifies correct Id in response
  - Verifies page count (100 for id=1, 200 for id=2, ..., 1000 for id=10)

## Running Tests

```bash
dotnet test
```

## Notes
- Tests run in parallel (configured in `xunit.runner.json`)
- Failed assertions log detailed information to test output
