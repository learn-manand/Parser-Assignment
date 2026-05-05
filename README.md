# Parser Assignment

## Overview

This project is a full-stack web application that parses structured data from unstructured email/text content.

It extracts tagged values (XML-like tags), validates input, and calculates tax-related values based on the extracted data.

---

## Tech Stack

### Backend

* .NET 9 Web API
* C#
* xUnit (Unit Testing)

### Frontend

* Angular 21 (Standalone)
* TypeScript

### DevOps

* Docker
* Docker Compose

---

## Features

### Backend

* Accepts raw `text/plain` input
* Extracts tagged fields:

  * `<cost_centre>`
  * `<total>`
  * `<payment_method>`
  * `<vendor>`
  * `<description>`
  * `<date>`
* Calculates:

  * Sales Tax
  * Total excluding tax
* Validations:

  * ❌ Missing closing tags → Reject
  * ❌ Missing `<total>` → Reject
  * ⚠ Missing `<cost_centre>` → Defaults to `UNKNOWN`
* Configurable Tax Rate via `appsettings.json`
* Unit tested using xUnit

---

### Frontend

* Paste email/text input
* Submit & Clear buttons
* Displays:

  * Parsed JSON response
  * Validation errors
  * Loading state
* Form validation (required input)

---

## Project Structure

```
Parser-Assignment/
│
├── backend/
│   ├── Parser.API
│   ├── Parser.Tests
│   └── ParserAssignment.sln
│
├── frontend/
│   └── parser-ui
│
├── docker-compose.yml
└── README.md
```

---

## Running with Docker

### Prerequisites

* Docker installed

### Run the application

```bash
docker compose up --build
```

---

## Access the Application

| Service  | URL                   |
| -------- | --------------------- |
| Frontend | http://localhost:4200 |
| Backend  | http://localhost:5000 |

---

## API Endpoint

```
POST /api/parser/parse
Content-Type: text/plain
```

### Sample Request

```
Hi Patricia,
<expense>
  <cost_centre>DEV632</cost_centre>
  <total>35,000</total>
  <payment_method>personal card</payment_method>
</expense>
```

---

## Configuration

### Backend Tax Rate

Configured in:

```
appsettings.json
```

```json
"TaxSettings": {
  "Rate": 0.10
}
```

---

### Frontend API URL

#### Development

```
src/environments/environment.ts
```

#### Production (Docker)

```
src/environments/environment.prod.ts
```

```ts
apiUrl: 'http://localhost:5000'
```

---

## Run Locally (Without Docker)

### Backend

cd backend/Parser.API
dotnet run

### Frontend

cd frontend/parser-ui
npm install
npm start

---

## Running Tests

```bash
cd backend
dotnet test
```

---

## E2E Testing

This test verifies the complete UI flow from input submission to parsed result display.

### Run tests

```bash
npx playwright test
```

---

## Notes

* Swagger is not used for testing due to `text/plain` input requirement.
* API is tested via frontend and Postman.
* Angular production build is used inside Docker.

---

## Author

**Anandha Kumar M**

---

## Submission Notes

This solution focuses on:

* Clean and testable backend logic
* Proper validation handling
* Real-world API design
* Dockerized full-stack deployment
* Maintainable frontend architecture

---
