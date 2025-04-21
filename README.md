# 📝 TodoApp

A clean, layered, and testable Todo application built with **ASP.NET Core**, following **Onion Architecture** principles.

---

## 📦 Technologies Used

- **ASP.NET Core 8.0**
- **Entity Framework Core (In-Memory & SQL Support)**
- **FluentValidation** – Input validation
- **xUnit** – Unit and Integration Testing
- **Moq** *(recommended for mocking)*
- **Onion Architecture** – With clear separation of concerns

---

## 🧱 Project Structure

TodoApp/ ├── API/ → API layer (Controllers, Middleware) ├── Application/ → Business logic & service interfaces ├── Core/ → Entities, Interfaces, Shared models ├── Infrastructure/ → EF Core, Repository implementations ├── TodoApp.Tests/ → Unit & integration tests (xUnit)

## 🚀 Features

- ✅ Create, Update, Delete, and Retrieve **Todo items**
- ✅ Each item belongs to a **Category**
- ✅ Input validation using **FluentValidation**
- ✅ Layered architecture (separation of API, Logic, Persistence)
- ✅ Integration Tests using In-Memory Database
- 🔜 Future improvements: JWT Auth, UI with React, CI/CD

---

## 🧪 Testing

Tests are located under `TodoApp.Tests/` and currently include:

- `GetAllAsync` test
- `CreateAsync` test using in-memory DB

> 🚧 More tests (e.g. for edge cases, failure scenarios, mocking repositories) are recommended.

---

## 🛠️ Getting Started

### 1. Clone the repo

```bash
git clone https://github.com/ali-tz-2004/TodoApp.git
cd TodoApp

cd API
dotnet run